using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace applyRequests.Models
{
    public class autoFacController:InterlDoActioncs
    {
        public void agree(string strProcessType, int intApplyRequestID)
        {
            throw new NotImplementedException();
        }

        public void agree(string strProcessType, int intApplyRequestID, DateTime? dTimePredictCompleteDate)
        {
            throw new NotImplementedException();
        }

        public flowRole bossRole(string strApplyUserID)
        {
            flowRole sample = new flowRole();
            sample.strBossID = "1";
            return sample;
        }

        public void complete(string strProcessType, int intApplyRequestID)
        {
            throw new NotImplementedException();
        }

        public void delete(int applyRequestID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<flowRole> listAllUsers()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<flowRole> listAllUsers(string bossID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<authorityRole> listAuthorityRoles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<flowRole> listRdAcceptTaskUsers()
        {
            throw new NotImplementedException();
        }

        public int loginPass(string strAccount, string strPassword)
        {
            throw new NotImplementedException();
        }

        public flowRole rdAcceptTaskUser(int intRequestID)
        {
            throw new NotImplementedException();
        }

        public void rdDispatch(string strProcessType, int intApplyRequestID, string strRdAcceptTaskUserID)
        {
            throw new NotImplementedException();
        }

        public flowRole rdDispatchRole()
        {
            throw new NotImplementedException();
        }

        public entityRequest readApplyRequest(int intApplyRequestID)
        {
            throw new NotImplementedException();
        }

        public flowRole readUserFlowType(string strApplyUserID)
        {
            throw new NotImplementedException();
        }

        public void reject(string strProcessType, int intApplyRequestID, string strRejectReason)
        {
            throw new NotImplementedException();
        }

        public bool saveApplyAuthority(string userID, string bossID, string gmail, string power, string department)
        {
            throw new NotImplementedException();
        }

        

        public void sendMail(int intApplyRequestID)
        {
            throw new NotImplementedException();
        }


        public void saveApplyRequest(string applyUser, DateTime applyDate, string title, string context)
        {
            throw new NotImplementedException();
        }
    }
}