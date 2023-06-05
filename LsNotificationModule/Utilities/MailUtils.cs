using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using LsSecurityModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mime;

namespace LsNotificationModule
{
    public class MailUtils
    {
        public static XafApplication application = null;
        public static bool enableNotifications = true;
        [Obsolete]
        public static void SendMail(string subject, string senderEMail, string senderName, string recipientEMail, string recipientName, string body)
        {
            string anomalies = string.Empty;
            anomalies += string.IsNullOrEmpty(subject) ? "*Subject is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(senderEMail) ? "*Sender e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(recipientEMail) ? "*Recipient e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(body) ? "*E-Mail body is empty\n" : string.Empty;
            if (string.IsNullOrEmpty(anomalies))
            {
                XPObjectSpace os = (XPObjectSpace)application.CreateObjectSpace();
                MailSettings _settings = MailSettings.GetInstance(os.Session);
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = _settings.port,
                    Host = _settings.host,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_settings.username, _settings.password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                MailMessage mailMsg = new MailMessage()
                {
                    Subject = subject,
                    Sender = new MailAddress(senderEMail, senderName),
                    From = new MailAddress(senderEMail, senderName),
                    Body = body
                };
                mailMsg.To.Add(new MailAddress(recipientEMail, recipientName));
                try
                {
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following errors\n{1}",
                        subject, ex.Message));
                }
            }
            else
                System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following anomalies\n{1}",
                    subject, anomalies));
        }
        public static void SendMail(string subject, string senderEMail, string senderName, string recipientEMail, string recipientName, string body, string signature)
        {
            string anomalies = string.Empty;
            anomalies += string.IsNullOrEmpty(subject) ? "*Subject is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(senderEMail) ? "*Sender e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(recipientEMail) ? "*Recipient e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(body) ? "*E-Mail body is empty\n" : string.Empty;
            if (string.IsNullOrEmpty(anomalies))
            {
                XPObjectSpace os = (XPObjectSpace)application.CreateObjectSpace();
                MailSettings _settings = MailSettings.GetInstance(os.Session);
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = _settings.port,
                    Host = _settings.host,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_settings.username, _settings.password),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                MailMessage mailMsg = new MailMessage()
                {
                    Subject = subject,
                    Sender = new MailAddress(senderEMail, senderName),
                    From = new MailAddress(senderEMail, senderName),
                    Body = body + (string.IsNullOrEmpty(signature) ? string.Empty : "\n\n" + signature)
                };
                mailMsg.To.Add(new MailAddress(recipientEMail, recipientName));
                try
                {
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following errors\n{1}",
                        subject, ex.Message));
                }
            }
            else
                System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following anomalies\n{1}",
                    subject, anomalies));
        }
        [Obsolete]
        public static void SendMail(string subject, string senderEMail, string senderName, string recipientEMail, string recipientName, string body, params Stream[] Attachments)
        {
            string anomalies = string.Empty;
            anomalies += string.IsNullOrEmpty(subject) ? "*Subject is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(senderEMail) ? "*Sender e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(recipientEMail) ? "*Recipient e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(body) ? "*E-Mail body is empty\n" : string.Empty;
            if (string.IsNullOrEmpty(anomalies))
            {
                XPObjectSpace os = (XPObjectSpace)application.CreateObjectSpace();
                MailSettings _settings = MailSettings.GetInstance(os.Session);
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = _settings.port,
                    Host = _settings.host,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_settings.username, _settings.password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = _settings.enableSSL,
                };
                MailMessage mailMsg = new MailMessage()
                {
                    Subject = subject,
                    Sender = new MailAddress(senderEMail, senderName),
                    From = new MailAddress(senderEMail, senderName),
                    Body = body
                };
                mailMsg.To.Add(new MailAddress(recipientEMail, recipientName));
                int i = 1;
                foreach (Stream s in Attachments)
                {
                    mailMsg.Attachments.Add(new Attachment(s, "Attachment" + i.ToString("D4")));
                    i++;
                }
                try
                {
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError sending mail : {1}", DateTime.Now, ex));
                }
            }
            else
                System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following anomalies\n{1}",
                    subject, anomalies));
        }

        public static void SendMail(string subject, string senderEMail, string senderName, string recipientEMail, string recipientName, string body, string signature, params Stream[] Attachments)
        {
            string anomalies = string.Empty;
            anomalies += string.IsNullOrEmpty(subject) ? "*Subject is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(senderEMail) ? "*Sender e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(recipientEMail) ? "*Recipient e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(body) ? "*E-Mail body is empty\n" : string.Empty;
            if (string.IsNullOrEmpty(anomalies))
            {
                XPObjectSpace os = (XPObjectSpace)application.CreateObjectSpace();
                MailSettings _settings = MailSettings.GetInstance(os.Session);
                SmtpClient smtpClient = new SmtpClient()
                {
                    Port = _settings.port,
                    Host = _settings.host,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_settings.username, _settings.password),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = _settings.enableSSL,
                };
                MailMessage mailMsg = new MailMessage()
                {
                    Subject = subject,
                    Sender = new MailAddress(senderEMail, senderName),
                    From = new MailAddress(senderEMail, senderName),
                    Body = body + (string.IsNullOrEmpty(signature) ? string.Empty : "\n\n" + signature)
                };
                mailMsg.To.Add(new MailAddress(recipientEMail, recipientName));
                int i = 1;
                foreach (Stream s in Attachments)
                {
                    mailMsg.Attachments.Add(new Attachment(s, "Attachment" + i.ToString("D4")));
                    i++;
                }
                try
                {
                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tError sending mail : {1}", DateTime.Now, ex));
                }
            }
            else
                System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following anomalies\n{1}",
                    subject, anomalies));
        }

        public static void SendMail(eMail mail)
        {
            string anomalies = string.Empty;
            anomalies += string.IsNullOrEmpty(mail.subject) ? "*Subject is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(mail.senderEmail) ? "*Sender e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(mail.recipientEMail) ? "*Recipient e-Mail is empty\n" : string.Empty;
            anomalies += string.IsNullOrEmpty(mail.body) ? "*E-Mail body is empty\n" : string.Empty;
            if (string.IsNullOrEmpty(anomalies))
            {
                Session session = mail.Session;
                try
                {
                    MailSettings _settings = MailSettings.GetInstance(session);
                    SmtpClient smtpClient = new SmtpClient()
                    {
                        Port = _settings.port,
                        Host = _settings.host,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(_settings.username, _settings.password),
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = _settings.enableSSL,

                    };

                    //AlternateView av = AlternateView.CreateAlternateViewFromString(mail.body, null, MediaTypeNames.Text.RichText);

                    MailMessage mailMsg = new MailMessage()
                    {
                        Subject = mail.subject,
                        Sender = new MailAddress(mail.senderEmail, mail.senderName),
                        From = new MailAddress(mail.senderEmail, mail.senderName),
                        Body = mail.body + (mail.signature != null ? "\n\n" + mail.signature.body : string.Empty)
                        //Body = mail.eMailTemplate != null ? string.Format(mail.eMailTemplate.body, GetParameters(mail.eMailTemplate, mail)) : string.Empty,
                    };

                    ContentType mimeType = new System.Net.Mime.ContentType("text/html");
                    // Add the alternate body to the message.
                    AlternateView alternate = AlternateView.CreateAlternateViewFromString(mail.body, mimeType);

                    mailMsg.AlternateViews.Add(alternate);
                    //Envoie d'email a une seule adresse
                    //mailMsg.To.Add(new MailAddress(mail.recipientEMail, mail.recipientName));

                    //Envoie d'email a plusieurs adresses
                    foreach (var address in mail.recipientEMail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        mailMsg.To.Add(address);
                    }

                    //mailMsg.Headers.Add("From", string.Format("{0} <{1}>", mail.senderName, mail.senderEmail));
                    foreach (FileAttachment fa in mail.fileAttachments)
                    {
                        MemoryStream s = new MemoryStream();
                        fa.File.SaveToStream(s);
                        s.Seek(0, SeekOrigin.Begin);
                        mailMsg.Attachments.Add(new Attachment(s, fa.File.FileName, "application/pdf"));
                    }

                    smtpClient.Send(mailMsg);
                    mail.sendingState = EMailState.Sent;
                }
                catch (Exception ex)
                {
                    mail.sendingState = EMailState.Failed;
                    System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following errors\n{1}",
                        mail.subject, ex.Message));
                }
                session.Save(mail);
                mail.Save();
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(string.Format("An e-mail with the subject \"{0}\" was not sent because of the following anomalies\n{1}",
                    mail.subject, anomalies));
            }
        }

        public static void CreateNotification(Session session, string id, LsSecuritySystemUser user, string subject, string body, DateTime dueDate, TimeSpan remindIn)
        {
            NotificationItem notificationItem = new NotificationItem(session)
            {
                id = id,
                subject = subject,
                content = body,
                dueDate = dueDate,
                RemindIn = remindIn,
                user = user,
            };
            notificationItem.Save();
        }

        public static eMail CreateEMail(IObjectSpace objectSpace, eMailTemplate template, LsSecuritySystemUser recipient, params object[] parameters)
        {
            Session session = ((XPObjectSpace)objectSpace).Session;
            LsSecuritySystemUser _recipient = objectSpace.GetObject<LsSecuritySystemUser>(recipient);
            Signature signature = template != null ? objectSpace.GetObject<Signature>(template.signature) : null;
            eMail mail = new eMail(session)
            {
                senderName = template != null ? template.senderName : "no name",
                senderEmail = template != null ? template.senderEmail : "no-reply@dummyCompany.com",
                recipient = _recipient,
                recipientName = recipient != null ? recipient.fullName : "no name",
                recipientEMail = recipient != null ? recipient.eMail : "null",
                replyTo = template != null ? template.replyTo : string.Empty,
                subject = template != null ? template.subject : "no subject",
                body = template != null ? string.Format(template.body, parameters) : string.Empty,
                signature = signature,
            };
            return mail;
        }
        public static eMail CreateEMail(IObjectSpace objectSpace, eMailTemplate template, string recipientEMail, string recipientName, params object[] parameters)
        {
            Session session = ((XPObjectSpace)objectSpace).Session;
            Signature signature = template != null ? objectSpace.GetObject<Signature>(template.signature) : null;
            eMail mail = new eMail(session)
            {
                senderName = template != null ? template.senderName : "no name",
                senderEmail = template != null ? template.senderEmail : "no-reply@dummyCompany.com",
                recipient = null,
                recipientName = recipientName,
                recipientEMail = recipientEMail,
                replyTo = template != null ? template.replyTo : string.Empty,
                subject = template != null ? template.subject : "no subject",
                body = template != null ? string.Format(template.body, parameters) : string.Empty,
                signature = signature,
            };
            return mail;
        }
        public static object[] GetParameters(eMailTemplate template, BaseObject currentObject)
        {
            object[] parameters = null;
            string propriety = string.Empty;
            if (!string.IsNullOrEmpty(template.bodyParameters))
            {
                System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tMessage parameters = {1}", DateTime.Now, template.bodyParameters));
                List<object> parameterValues = new List<object>();
                string[] parameterNames = template.bodyParameters.Split(';');
                foreach (string parameter in parameterNames)
                {

                    if (parameter.StartsWith("$") || parameter.StartsWith(" $"))
                    {
                        propriety = parameter;
                        if (parameter.StartsWith(" "))
                        {
                            propriety = parameter.TrimStart("".ToCharArray());
                        }
                        
                        System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tParameter is a property : ", DateTime.Now, propriety));
                        if (propriety.Contains('.'))
                        {
                            System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tParameter is a nested property. Getting value ...", DateTime.Now));
                            string[] memberNames = propriety.Substring(1).Split('.');
                            XPBaseObject obj = currentObject;
                            for (int i = 0; i < memberNames.Length - 1; i++)
                            {
                                obj = (XPBaseObject)obj.GetMemberValue(memberNames[i]);
                            }
                            parameterValues.Add(obj.GetMemberValue(memberNames[memberNames.Length - 1]));
                        }
                        else
                            parameterValues.Add(currentObject.GetMemberValue(propriety.Substring(1)));
                    }
                    else
                        parameterValues.Add(propriety);
                }
                parameters = parameterValues.ToArray();
            }
            return parameters;
        }
        public static eMail CreateEMail(IObjectSpace objectSpace, eMailTemplate template, string recipientEMail, string recipientName, BaseObject obj)
        {
            Session session = ((XPObjectSpace)objectSpace).Session;
            Signature signature = template != null ? objectSpace.GetObject<Signature>(template.signature) : null;
            eMail mail = new eMail(session)
            {
                senderName = template != null ? template.senderName : "no name",
                senderEmail = template != null ? template.senderEmail : "no-reply@dummyCompany.com",
                recipient = null,
                recipientName = recipientName,
                recipientEMail = recipientEMail,
                replyTo = template != null ? template.replyTo : string.Empty,
                subject = template != null ? template.subject : "no subject",
                body = template != null ? string.Format(template.body, GetParameters(template, obj)) : string.Empty,
                signature = signature,
            };
            return mail;
        }
        public static void ProcessNotification(IObjectSpace objectSpace, NotificationSetting ns, bool commitChanges)
        {
            XPCollection objects = new XPCollection(PersistentCriteriaEvaluationBehavior.InTransaction, ns.Session, ns.Session.GetClassInfo(ns.objectType), CriteriaOperator.Parse(ns.condition));
            foreach (XPBaseObject obj in objects)
            {
                string id = string.Format("{0} : {1}", ns.id, obj);
                XPCollection<NotificationItem> notificationItems = new XPCollection<NotificationItem>(PersistentCriteriaEvaluationBehavior.InTransaction,
                    ns.Session, CriteriaOperator.Parse("id = ?", id));
                if (notificationItems.Count == 0)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tCreating notifications...", DateTime.Now));
                    System.Diagnostics.Trace.WriteLine(string.Format("{0:dd.MM.yy hh:mm:sss.fff}\tObtaining message parameters...", DateTime.Now, id));
                    object[] parameters = MailUtils.GetParameters(ns.mailTemplate, (BaseObject)obj);
                    string notificationBody = parameters != null ? string.Format(ns.mailTemplate.body, parameters) : ns.mailTemplate.body;
                    foreach (NotificationUser nu in ns.users)
                    {
                        MailUtils.CreateNotification(ns.Session, id, nu.user, ns.mailTemplate.subject, notificationBody, DateTime.Now, new TimeSpan(0));
                        if (nu.sendMail)
                        {
                            string recipientMail = nu.user != null ? nu.user.eMail : "";
                            string recipientName = nu.user != null ? nu.user.fullName : "";
                            eMail mail = MailUtils.CreateEMail(objectSpace, ns.mailTemplate, nu.user, recipientMail, recipientName, null);
                            MailUtils.SendMail(mail);
                        }
                    }
                }
            }
            if (commitChanges)
                ns.Session.CommitTransaction();
        }
        public static void ProcessEMails(IObjectSpace objectSpace)
        {
            Session session = ((XPObjectSpace)objectSpace).Session;
            XPCollection<eMail> emails = new XPCollection<eMail>(session, CriteriaOperator.Parse("sendingState = ? and scheduledDate >= ?", EMailState.ToSend, DateTime.Today));
            if (emails.Count > 0)
            {
                foreach (eMail email in emails)
                    MailUtils.SendMail(email);
            }
        }
    }
}
