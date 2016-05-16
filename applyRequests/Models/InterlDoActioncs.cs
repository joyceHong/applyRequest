using System;
namespace applyRequests.Models
{
    interface InterlDoActioncs
    {
        void agree(string strProcessType, int intApplyRequestID);
        void agree(string strProcessType, int intApplyRequestID, DateTime? dTimePredictCompleteDate);
        flowRole bossRole(string strApplyUserID);
        void complete(string strProcessType, int intApplyRequestID);
        void delete(int applyRequestID);
        System.Collections.Generic.IEnumerable<flowRole> listAllUsers();
        System.Collections.Generic.IEnumerable<flowRole> listAllUsers(string bossID);
        System.Collections.Generic.IEnumerable<authorityRole> listAuthorityRoles();
        System.Collections.Generic.IEnumerable<flowRole> listRdAcceptTaskUsers();
        int loginPass(string strAccount, string strPassword);
        flowRole rdAcceptTaskUser(int intRequestID);
        void rdDispatch(string strProcessType, int intApplyRequestID, string strRdAcceptTaskUserID);
        flowRole rdDispatchRole();
        entityRequest readApplyRequest(int intApplyRequestID);
        flowRole readUserFlowType(string strApplyUserID);
        void reject(string strProcessType, int intApplyRequestID, string strRejectReason);
        bool saveApplyAuthority(string userID, string bossID, string gmail, string power, string department);
        void saveApplyRequest(string applyUser, DateTime applyDate,string title, string context);
        void sendMail(int intApplyRequestID);
    }
}
