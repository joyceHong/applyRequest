using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace applyRequests.Models
{
    public class controlEmail
    {
        public void sendMail(entityRequest entityRequestObj, flowRole flowRoleUser)
        {
            try
            {

                if (flowRoleUser == null)
                {
                    return;
                }

                if (flowRoleUser.strEmail == null)
                {
                    return;
                }

                commonSend(entityRequestObj, flowRoleUser.strEmail, flowRoleUser.strRoleUserName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void sendMail(entityRequest entityRequestObj, string strSendEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(strSendEmail))
                {
                    return;
                }

                commonSend(entityRequestObj, strSendEmail,"");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void commonSend(entityRequest entityRequestObj, string strEndMail, string strRoleUserName)
        {
            MailMessage message = new MailMessage("drcooper08@gmail.com", strEndMail);
            //MailMessage message = new MailMessage("joyceHongzoom@gmail.com", strEndMail);
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;

            string applyResult = "";
            string strContext = "";
            switch (entityRequestObj.processAction.Trim())
            {
                case "complete":
                    applyResult = "已同意";
                    break;
                case "reject":
                    applyResult = "已退回";
                    break;
                case "end":
                    applyResult = entityRequestObj.completeDate.Value.ToString("yyyy/MM/dd") + "已完工";
                    break;
                case "rdDispatch":
                    applyResult = "送至RD窗口";
                    break;
                case "rdAcceptTaskUser":
                    applyResult = "送至RD處理人員" + strRoleUserName;
                    break;
            }

            message.Subject = "申請人:" + entityRequestObj.applyUserName + " 提出需求日期 : " + entityRequestObj.requestDate.Value.ToString("yyyy/MM/dd") + "  回覆結果:" + applyResult;

            strContext += "申請人: " + entityRequestObj.applyUserName + "<br>";
            strContext += "申請日期: " + entityRequestObj.requestDate.Value.ToString("yyyy/MM/dd") + "<br>";
            strContext += "申請理由: " + entityRequestObj.applyReason + "<br>";
            strContext += "流程: " + entityRequestObj.processName + "<br>";
            strContext += "申請結果: " + applyResult + "<br>";


            if (entityRequestObj.predictCompleteDate != null)
            {
                strContext += "預計完工日: " + entityRequestObj.predictCompleteDate.Value.ToString("yyyy/MM/dd");
            }



            message.Body = strContext;

            var smtpClient = new SmtpClient("smtp.gmail.com", 25)
            {
                EnableSsl = true,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("drcooper08@gmail.com", "16145679"),
                //Credentials = new NetworkCredential("joyceHongzoom@gmail.com", "fxnjzuoddangdbwt"),
            };
            smtpClient.EnableSsl = true;
            smtpClient.Send(message);
        }
       
    }
}