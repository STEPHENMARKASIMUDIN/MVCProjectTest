using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DebitCreditMemo.Models {
    public class GenerateSeriesMemoModel {
        public string MemoSeries { get; set; }
        public string IRNumber { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDate1 { get; set; }
        public string TransactionDate2 { get; set; }
        public string TransactionDate3 { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }
        public string Reason3 { get; set; }
        public string ReferenceNo1 { get; set; }
        public string ReferenceNo2 { get; set; }
        public string ReferenceNo3 { get; set; }
        public string WalletUser { get; set; }
        public string WalletID { get; set; }
        public decimal WrongAmount { get; set; }
        public decimal CorrectAmount { get; set; }
        public decimal AdjustedAmount { get; set; }
        public string OperatorName { get; set; }
        public string NotedBy { get; set; }
        public string AuditedBy { get; set; }
        public string ReceivedBy { get; set; }
        public string Created_at { get; set; }
        public string Updated_at { get; set; }
        public int Status { get; set; }
    }
}