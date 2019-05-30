using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;

using EncodedQueryTree.Base;
using Matrix.Components.MatrixExpression;
using Matrix.Data;
using Matrix.DataAccessLayer.Components;
using Matrix.DataAccessLayer.Data;
using Matrix.Multilingual.Components;
using System.Collections.Generic;

namespace Matrix.Components.MatrixMail
{
	/// <summary>
	/// Abstract base class for HTML message makers. This class is responsible
	/// for generating the final form of the email message, taking into account the text
	/// that the user entered, the Jips templates used for adding extra content,
	/// the delivered-by text, and the antispam unsubscribe link.
	/// </summary>
	public abstract class HTMLMessageMaker : MatrixExpressionRecord, IHTMLMessageMaker, IMultilingualItemContainer
	{
		protected static readonly IDALLibrary Lib = DALLibrary.Instance; 
		private readonly MatrixEmail m_oEmail;
		private readonly RenderVarCollection m_RenderVars;
		private static readonly Regex m_rgxStringHTML = new Regex( "\\</?[_a-zA-Z][^<>]*>", RegexOptions.Compiled | RegexOptions.Multiline );

		public const string ROSTER_FIRSTNAME = "FirstName";
		public const string ROSTER_LASTNAME = "LastName";
		public const string ROSTER_FULLNAME = "FullName";
		public const string ROSTER_EMAIL = "Email";
		public const string ROSTER_AGENTPHONE = "AgentPhone";
		public const string ROSTER_AGENTOFFICENAME = "OfficeName";

		/// <summary>
		/// Optional prefix to add to the subject line
		/// </summary>
		public string SubjectPrefix { get; set; }
		public MatrixEmail MatrixEmail { get { return m_oEmail; } }
		public RenderVarCollection RenderVars { get { return m_RenderVars; } }
		private bool m_bRenderingHTML;

		protected HTMLMessageMaker( MatrixEmail oEmail )
		{
			m_oEmail = oEmail;
			m_RenderVars = new RenderVarCollection( this );
		}

		/// <summary>
		/// Gets an HTML version of the email body.
		/// </summary>
		/// <param name="strEmailAddress">Email address for unsubscribe</param>
		/// <returns>HTML text</returns>
		public string MakeMessageHTML( string strEmailAddress )
		{
			m_bRenderingHTML = true; // this is not pretty
			string strBody = GetMessageBody();
			string strPreview = strBody.Substring(0, 30); //TODO set a way better preview
			string strUnsubscribeFragment = MakeUnsubscribeFragment(strEmailAddress);
			string strDeliveredBy = DeliveredByText.Replace("\r\n", "<br/>");

			string strHTMLMessage = string.Format(HTML_MESSAGE_TEMPLATE, strPreview, strBody, strDeliveredBy, strUnsubscribeFragment);

			return strHTMLMessage;
		}

		/// <summary>
		/// Gets a plain text version of the email body.
		/// </summary>
		/// <param name="strEmailAddress">Email address for unsubscribe</param>
		/// <returns>text</returns>
		public string MakeMessageText( string strEmailAddress )
		{
			m_bRenderingHTML = false; // this is not pretty
			string strBody = GetMessageBody();
			string strUnsubscribeURL = MakeUnsubscribeURL( strEmailAddress );
			string strUnsubscribeTextBody = "";
			if (!String.IsNullOrEmpty( strUnsubscribeURL ))
			{
				// Add unsubscribe URL line to message if the URL is not empty.
				string strUnsubscribeText = UnsubscribeText;
				strUnsubscribeTextBody = strUnsubscribeText + "\r\n" + strUnsubscribeURL;
			}
			
			string strTextMessage = StripHtml( strBody )
				+ "\r\n\r\n"
				+ DeliveredByText + "\r\n\r\n"
				+ strUnsubscribeTextBody;
			return strTextMessage;
		}

		public abstract string GetSubject();

		/// <summary>
		/// Make a deep link URL with the given params (a la PortalURL).
		/// The URL is handled by EmailDeepLinkController.
		/// </summary>
		public static string MakeDeepLink( string strParams )
		{
			return Lib.MatrixRegistry.GetStringEntry( Const.MR_MATRIX_BASE_URL, "http://localhost/Matrix/" ) + "EmailLink" + strParams;
		}

		public static string MakeRenewLisitingLink(string strParams)
		{
			return Lib.MatrixRegistry.GetStringEntry(Const.MR_MATRIX_BASE_URL, "http://localhost/Matrix/") + "AutoEmailRenew" + strParams;
		}

		/// <summary>
		/// Default signature for the impersonating user
		/// </summary>
		public static string DefaultSignature( UserInfo UInfo )
		{
			return DefaultSignature( UInfo.ImpersonatingFullName, UInfo.ImpersonatingUserID );
		}

		/// <summary>
		/// Default signature for the user with the specified name and ID.
		/// </summary>
		public static string DefaultSignature( string strName, int nID )
		{
			var uinfoSystem = UserInfo.GetSystemUserInfo();
			string strPhone = "";
			string strOffice = "";
			var cc = new ColumnCollection( Lib, true, null, uinfoSystem );

			int nPhoneField = Lib.Aliases.GetAlias( Aliases.AgentPhone, Lib.Tables.CROSS_ROSTER_TABLE_ID );
			if( nPhoneField != Const.ERROR )
			{
				cc.AddColumn( nPhoneField );
			}
			int nOfficeNameField = Lib.Aliases.GetAlias( Aliases.AgentOfficeName, Lib.Tables.CROSS_ROSTER_TABLE_ID );
			if( nOfficeNameField != Const.ERROR )
			{
				cc.AddColumn( nOfficeNameField );
			}
			if( nPhoneField != Const.ERROR || nOfficeNameField != Const.ERROR )
			{
				DataTable dtRoster = Lib.DBRoster.GetCRosterDetails( nID, cc );
				if (dtRoster.Rows.Count > 0)
				{
					var drRoster = dtRoster.Rows[0];
					if (nPhoneField != Const.ERROR)
					{
						strPhone = drRoster.Field<string>( Lib.Fields.GetFieldName( nPhoneField ) );
					}
					if (nOfficeNameField != Const.ERROR)
					{
						strOffice = drRoster.Field<string>( Lib.Fields.GetFieldName( nOfficeNameField ) );
					}
				}
			}
			return String.Format( "{0}\r\n{1}\r\n{2}\r\n", strName, strPhone, strOffice );
		}

		public static Dictionary<string, string> GetRosterInfo(int nID)
		{
			Dictionary<string, string> dict = new Dictionary<string, string>();
			dict.Add(ROSTER_FIRSTNAME, "");
			dict.Add(ROSTER_LASTNAME, "");
			dict.Add(ROSTER_FULLNAME, "");
			dict.Add(ROSTER_EMAIL, "");
			dict.Add(ROSTER_AGENTPHONE, "");
			dict.Add(ROSTER_AGENTOFFICENAME, "");
			UserInfo uinfoSystem = UserInfo.GetSystemUserInfo();
			ColumnCollection cc = new ColumnCollection(Lib, true, null, uinfoSystem);
			int nFirstNameField = Lib.Aliases.GetAlias(Aliases.FirstName, Lib.Tables.CROSS_ROSTER_TABLE_ID);
			int nLastNameField = Lib.Aliases.GetAlias(Aliases.LastName, Lib.Tables.CROSS_ROSTER_TABLE_ID);
			int nEmailField = Lib.Aliases.GetAlias(Aliases.Email, Lib.Tables.CROSS_ROSTER_TABLE_ID);
			int nPhoneField = Lib.Aliases.GetAlias(Aliases.AgentPhone, Lib.Tables.CROSS_ROSTER_TABLE_ID);
			int nOfficeNameField = Lib.Aliases.GetAlias(Aliases.AgentOfficeName, Lib.Tables.CROSS_ROSTER_TABLE_ID);

			foreach (int nFieldID in new int[] { nFirstNameField, nLastNameField, nEmailField, nPhoneField, nOfficeNameField })
			{
				if (nFieldID != Const.ERROR)
				{
					cc.AddColumn(nFieldID);
				}
			}

			DataTable dtRoster = Lib.DBRoster.GetCRosterDetails(nID, cc);

			DataColumn dcFirstName = nFirstNameField == Const.ERROR ? null : dtRoster.Columns[Lib.Fields.GetFieldName(nFirstNameField)];
			DataColumn dcLastName = nLastNameField == Const.ERROR ? null : dtRoster.Columns[Lib.Fields.GetFieldName(nLastNameField)];
			DataColumn dcEmail = nEmailField == Const.ERROR ? null : dtRoster.Columns[Lib.Fields.GetFieldName(nEmailField)];
			DataColumn dcPhone = nPhoneField == Const.ERROR ? null : dtRoster.Columns[Lib.Fields.GetFieldName(nPhoneField)];
			DataColumn dcOfficeName = nOfficeNameField == Const.ERROR ? null : dtRoster.Columns[Lib.Fields.GetFieldName(nOfficeNameField)];
			if (dtRoster.Rows.Count > 0)
			{
				DataRow dr = dtRoster.Rows[0];
				dict[ROSTER_FIRSTNAME] = dcFirstName == null ? "" : dr[dcFirstName].ToString();
				dict[ROSTER_LASTNAME] = dcLastName == null ? "" : dr[dcLastName].ToString();
				dict[ROSTER_FULLNAME] = dict[ROSTER_FIRSTNAME] + " " + dict[ROSTER_LASTNAME];
				dict[ROSTER_EMAIL] = dcEmail == null ? "" : dr[dcEmail].ToString();
				dict[ROSTER_AGENTPHONE] = dcPhone == null ? "" : dr[dcPhone].ToString();
				dict[ROSTER_AGENTOFFICENAME] = dcOfficeName == null ? "" : dr[dcOfficeName].ToString();
			}
			return dict;

		}

		public static string GetDeliveredByText( IMultilingualItemContainer container, Language lang )
		{
			return new MLString( container, 3511, "Delivered By CoreLogic, Inc. | 40 Pacifica, Irvine, CA 92618" ).GetMLValue( lang );
		}

		protected bool RenderingHTML
		{
			get { return m_bRenderingHTML; }
		}

		protected Language Lang { get { return m_oEmail.Language; } }

		protected string LanguageParam
		{
			get { return DBLanguage.IsUnilingualSystem() ? "" : "&L=" + (int) Lang; }
		}

		protected abstract string GetMessageBody();

		protected abstract string MakeUnsubscribeURL( string strEmailAddress );

		protected static string MakeID( int nID, int nKey )
		{
			return nID + nKey.ToString().PadLeft( Const.EMAILKEYLENGTH, '0' );
		}

		protected static string ValidateFormula( EmailTemplateType type, string strFormula )
		{
			try
			{
				CompileFormula( type, strFormula );
				return null;
			}
			catch( JipsException err )
			{
				return err.Message;
			}
		}

		protected string EvalTemplate( string strCacheKey, Func<MatrixString> GetTemplate )
		{
			var msTemplate = HttpRuntime.Cache[ strCacheKey ] as MatrixString;
			if( msTemplate == null )
			{
				msTemplate = GetTemplate();
				HttpRuntime.Cache.Insert( strCacheKey, msTemplate, DBEmailTexts.GetEmailTemplateDependency() );
			}
			return msTemplate.ToString( this );
		}

		protected static MatrixString CompileFormula( EmailTemplateType type, string strFormula )
		{
			var eb = new ExpressionBuilder( new EmailContainer( type ) );
			return eb.ParseStringToken( Jips.EESToJips( strFormula ) );
		}

		private string UnsubscribeText
		{
			get { return new MLString( this, 3521, "Click this link if you wish to Unsubscribe." ).GetMLValue( Lang ); }
		}

		private string MakeUnsubscribeFragment(string strEmail)
		{
			string strUnsubscribeURL = MakeUnsubscribeURL(strEmail);
			return string.IsNullOrWhiteSpace(strUnsubscribeURL) ? "" : string.Format(UNSUBSCRIBE_FRAGMENT, strUnsubscribeURL, UnsubscribeText);
		}

		public static string StripHtml( string strMessage )
		{
			return m_rgxStringHTML.Replace( strMessage, "" );
		}

		private string DeliveredByText
		{
			get { return GetDeliveredByText( this, Lang ); }
		}

		private class EmailContainer : IMatrixExpressionProvider
		{
			private readonly EmailTemplateType m_type;

			public EmailContainer( EmailTemplateType type )
			{
				m_type = type;
			}

			void IMatrixExpressionProvider.GetFieldTypeAndColumnName( string strFieldName, out MatrixTypes mtFieldDataType, out IComparable medkFieldName )
			{
				mtFieldDataType = GetFieldType( strFieldName );
				medkFieldName = strFieldName;
			}

			private MatrixTypes GetFieldType( string strKey )
			{
				switch( ( strKey ?? "" ).ToLower() )
				{
					case "agentemailsignature":
					case "agentfullname":
					case "agentemail":
					case "message":
					case "subject":
					case "portalhomepagelink":
					case "temporarypassword":
					case "optinurl":
					case "optouturl":
					case "recipientemail":
					case "recipientname":
					case "agentoptinurl":
						return MatrixTypes.MatrixString;
					case "changedlistingscount":
					case "newlistingscount":
					case "listinglimit":
						if( m_type == EmailTemplateType.AutoEmail )
						{
							return MatrixTypes.MatrixInt;
						}
						break;
					case "agentphone":
					case "agentofficename":
					case "fulllistingslink":
					case "newlistingslink":
					case "publishcomments":
					case "searchcriteria":
					case "name_last":
					case "name_first":
					case "displaycontent":
					case "salutation":
					case "renewlistinglink":
						if( m_type == EmailTemplateType.AutoEmail || m_type == EmailTemplateType.OptinWhitelist )
						{
							return MatrixTypes.MatrixString;
						}
						break;
					case "deeplink":
						return MatrixTypes.MatrixString;
					case "contentlink":
					case "contentname":
						if( m_type == EmailTemplateType.DirectEmail || m_type == EmailTemplateType.ReverseProspectingEmail )
						{
							return MatrixTypes.MatrixString;
						}
						break;
					case "mlnumber":
						if( m_type == EmailTemplateType.ReverseProspectingEmail )
						{
							return MatrixTypes.MatrixString;
						}
						break;
				}
				throw new JipsException( strKey + " is not a valid field for template type " + m_type, 0 );
			}
		}
		private const string HTML_MESSAGE_TEMPLATE = @"<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"" lang=""en"">

<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"">
	<meta charset=""UTF-8"">
	<meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
	<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
	<title>Email Alert</title>
	<style type=""text/css"">
		.pb-2 {{
			padding-bottom: 20px;
		}}

		a:focus {{
			-webkit-box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.15);
			box-shadow: 0 0 0 3px rgba(0, 0, 0, 0.15);
			outline: none;
		}}

		a[x-apple-data-detectors] {{
			text-decoration: none !important;
			font-size: inherit !important;
			font-family: inherit !important;
			font-weight: inherit !important;
			line-height: inherit !important;
		}}

		@media only screen and (max-width: 480px) {{

			body,
			table,
			td,
			p,
			a,
			li,
			blockquote {{
				-webkit-text-size-adjust: none !important;
			}}

			body {{
				width: 100% !important;
				min-width: 100% !important;
			}}

			table {{
				width: 100%;
			}}

			h1 {{
				line-height: 125% !important;
			}}

			h2 {{
				font-size: 20px !important;
				line-height: 125% !important;
			}}

			h3 {{
				font-size: 18px !important;
				line-height: 125% !important;
			}}

			h4 {{
				font-size: 16px !important;
				line-height: 125% !important;
			}}

			p {{
				font-size: 18px !important;
				line-height: 125% !important;
				mso-line-height-rule: exactly;
			}}

			.pb-2 {{
				padding-bottom: 0;
			}}
		}}
	</style>
	<!-- CIRCLE AVATAR -->
	<style type=""text/css"">
		.hide {{
			display: block;
		}}
	</style>
	<!--[if (mso)|(IE)]>
			<xml:namespace ns=""urn:schemas-microsoft-com:vml"" prefix=""v"" />
			<style>v\: * {{ behavior:url(#default#VML); display:inline-block }}</style>
			<!<![endif]-->
	<!--[if (gte mso 9)|(IE)]>
			<style>
			.hide {{
				display: none;
			}}
			</style>
			<![endif]-->

	<!-- // CIRCLE AVATAR -->

</head>

<body leftmargin=""0"" marginwidth=""0"" topmargin=""0"" marginheight=""0"" offset=""0"" style='height: 100%; width: 100%; color: #3a3a3a; font-size: 16px; font-family: ""geomanist-light"", ""Helvetica"", Arial, sans-serif, ""Apple Color Emoji"", ""Segoe UI Emoji"", ""Segoe UI Symbol""; font-weight: 300; line-height: 25px; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0; padding: 0;'
	bgcolor=""#f1f1f1"">
	<!--PREVIEW TEXT-->
	<span style=""display: none !important; font-size: 0px; line-height: 0px; max-height: 0px; max-width: 0px; opacity: 0; overflow: hidden; visibility: hidden; mso-hide: all;"">"
+ "{0}" //preview
+ @"</ span >

	< !-- // PREVIEW TEXT-->

	< div class=""pb-2""></div>
	<table role = ""main"" class=""table"" align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" height=""100%"" width=""100%""
		style='border-collapse: collapse; height: 100%; width: 100%; color: #3a3a3a; font-size: 16px; font-family: ""geomanist-light"", ""Helvetica"", Arial, sans-serif, ""Apple Color Emoji"", ""Segoe UI Emoji"", ""Segoe UI Symbol""; font-weight: 300; line-height: 25px; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; margin: 0; padding: 0;'
		bgcolor=""#f1f1f1"">
		<tr>
			<td align = ""center"" valign=""top"" style='height: 100%; width: 100%; color: #3a3a3a; font-size: 16px; font-family: ""geomanist-light"", ""Helvetica"", Arial, sans-serif, ""Apple Color Emoji"", ""Segoe UI Emoji"", ""Segoe UI Symbol""; font-weight: 300; line-height: 25px; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border-top-width: 0; margin: 0; padding: 0;'
				bgcolor=""rgba(235, 235, 235, 0.25)"">
			</td>
		</tr>
		<tr>
			<td align = ""center"" valign=""top"" style=""mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">

				<table border = ""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border-top-width: 0; border-bottom-width: 0;""
					bgcolor=""#f1f1f1"">
					<tr>
						<td align = ""center"" valign=""top"" style=""mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">
							<table border = ""0"" cellpadding=""0"" cellspacing=""0"" width=""600"" style=""border-collapse: separate !important; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border-radius: 0;""
								bgcolor=""#FFFFFF"">
								<tr>
									<td align = ""center"" valign=""top"" style=""mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; border: 1px solid #d2d2d2;"">"
+ "{1}" //message body - extra tables below are in this table cell, which closes below

+ @"									<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
											style=""min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">
											<tbody>
												<tr>
													<td valign=""top"" style=""padding: 18px 25px; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #6B6B6B;"">
														<table align=""left"" border=""0"" cellpadding=""0""
															cellspacing=""0"" style=""max-width: 100%; min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;""
															width=""100%"">
															<tbody>
																<tr>
																	<td valign=""top"" style=""mso-line-height-rule: exactly; line-height: 125%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word; font-size: 16px;""
																		align=""left"">

																		<!-- SUPPORT FOOTER -->
																		<div style=""color: #ffffff; font-size:12px; line-height: 125%; font-family: geomanist-book, Helvetica, Arial; font-weight: 400;""
																			align=""center"">"
+ "{2}" //delivered by
+ @"
																		</div>"
+ "{3}" //unsubscribe fragment
+ @"
																		<!-- // SUPPORT FOOTER -->

																	</td>
																</tr>
															</tbody>
														</table>

													</td>
												</tr>
											</tbody>
										</table>

										<table border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%""
											style=""min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">
											<tbody>
												<tr>
													<td valign=""top"" style=""padding: 18px 25px; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; background-color: #ffffff;"">
														<table align=""left"" border=""0"" cellpadding=""0""
															cellspacing=""0"" style=""max-width: 100%; min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;""
															width=""100%"">
															<tbody>
																<tr>
																	<td valign=""top"" style=""mso-line-height-rule: exactly; line-height: 125%; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word; font-size: 16px;""
																		align=""left"">
																		<!-- EMAIL FOOTER -->
																		<table border=""0"" cellpadding=""0""
																			cellspacing=""0"" width=""100%""
																			style=""min-width: 100%; border-collapse: collapse; mso-table-lspace: 0pt; mso-table-rspace: 0pt; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">
																			<tbody>
																				<tr>
																					<td valign=""bottom""
																						align=""right"" style=""text-align:right; vertical-align: bottom; color:#6B6B6B; font-size:14px; line-height: 20px; height:36px; font-family: geomanist-light, Helvetica Neue, Helvetica, Arial, sans-serif; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; word-break: break-word; width:49.5%; padding-right:8px; border-right:2px solid #6B6B6B"">
																						Powered By
																					</td>
																					<td valign=""top"" style=""padding-left:8px; height:36px;"">
																						<img src=""src/cl-logo-h.png""
																							border=""0""
																							width=""127""
																							height=""36"" alt=""CoreLogic Inc.""
																							style=""display: block;"" />
																					</td>
																				</tr>
																			</tbody>
																		</table>
																		<!-- // EMAIL FOOTER -->

																	</td>
																</tr>
															</tbody>
														</table>

													</td>
												</tr>
											</tbody>
										</table>
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</body>

</html>";

		private const string UNSUBSCRIBE_FRAGMENT = @"<div style=""color:#ffffff; font-size:12px; font-family: geomanist-book, Helvetica, Arial; font-weight: 400;""
																			align=""center"">
																			<a title=""Unsubscribe"" href="""
+ "{0}" //unsubscribe url
+ @"""
																				target=""_self"" style=""letter-spacing: normal; line-height: 100%; text-align: center; color: #FFFFFF !important; mso-line-height-rule: exactly; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;"">
																				<font color=""#ffffff"">"
+ "{1}" //unsubscribe text
+ @"
																				</font>
																			</a>
																		</div>";
	}
}
