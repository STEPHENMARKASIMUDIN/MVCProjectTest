using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DebitCreditMemo.Models {
    public class HomeViewModel {
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode = true)]
        public DateTime date { get; set; }
        [Display(Name = "Debit/Credit Memo #")]
        public string DebtCredNo { get; set; }
        [Display(Name = "IR #")]
        [Required(ErrorMessage = "IR Number is required")]
        public string IRNo { get; set; }
        public string DebitCredit { get; set; }
        public bool WrongAmount {get;set;}
        public bool Erroneous{ get; set; }
        public bool SystemError{ get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string TransactionDate1 { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string TransactionDate2 { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string TransactionDate3 { get; set; }
        public string refno1 { get; set; }
        public string refno2 { get; set; }
        public string refno3 { get; set; }

        [Display(Name = "Wallet Username")]
        [Required(ErrorMessage = "Wallet Username is required")]
        public string WalletUser { get; set; }
        [Display(Name = "Wallet ID #")]
        [Required(ErrorMessage = "Wallet ID is required")]
        public string WalletID { get; set; }
        [Display(Name = "Incorrect Amount")]
        [Required(ErrorMessage = "Incorrect Amount is required")]
        public Decimal IncorrectAmount { get; set; }
        [Display(Name = "Correct Amount")]
        [Required(ErrorMessage = "Correct Amount is required")]
        public Decimal CorrectAmount { get; set; }
        [Display(Name = "Amount to be adjusted")]
        [Required(ErrorMessage = "Amount to be Adjusted is required")]
        public Decimal AdjustAmount { get; set; }

        public string Created_at { get; set; }
        public string MemoSeries { get; set; }
        public string OperatorName { get; set; }
        [Display(Name = "Attach File")]
        public string File { get; set; }
        public string FileValue { get; set; }
        public string Fullname { get; set; }
        public string Approver1 { get; set; }
        public string Approver2 { get; set; }
        public string Approver3 { get; set; }
        public string FileType { get; set; }
        public string Noted_at { get; set; }
        public string Audited_at { get; set; }
        public string Received_at { get; set; }
    }

    public class UpdateMemoModel
    {
        public string MemoSeries { get; set; }
        public int Status { get; set; }
        public string Fullname { get; set; }
        public string OperatorName { get; set; }
        public string IRNumber { get; set; }
    }

    public class CreateMemoModel
    {
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
        public string Created_at { get; set; }
        public string MemoSeries { get; set; }
        public string File { get; set; }
        public int Status { get; set; }
        public string Fullname { get; set; }
        public string FileType { get; set; }
        public string Noted_at { get; set; }
        public string Audited_at { get; set; }
        public string Received_at { get; set; }
    }

    public class RequestDetails
    {
        public string AdjustedAmount { get; set; }
        public string Auditedby { get; set; }
        public string CorrectAmount { get; set; }
        public string Created_at { get; set; }
        public string Email { get; set; }
        public string Fullname { get; set; }
        public string IRNumber { get; set; }
        public string Level { get; set; }
        public string MemoSeries { get; set; }
        public string NotedBy { get; set; }
        public string OperatorName { get; set; }
        public string Reason1 { get; set; }
        public string Reason2 { get; set; }
        public string Reason3 { get; set; }
        public string ReceivedBy { get; set; }
        public string ReferenceNo1 { get; set; }
        public string ReferenceNo2 { get; set; }
        public string ReferenceNo3 { get; set; }
        public string Status { get; set; }
        public string TransactionDate1 { get; set; }
        public string TransactionDate2 { get; set; }
        public string TransactionDate3 { get; set; }
        public string TransactionType { get; set; }
        public string Updated_at { get; set; }
        public string WalletID { get; set; }
        public string WalletUser { get; set; }
        public string WrongAmount { get; set; }
        public string File { get; set; }
        public string FileType { get; set; }
        public string Noted_at { get; set; }
        public string Audited_at { get; set; }
        public string Received_at { get; set; }
    }

    public class Signatories
    {
        public int Level { get; set; }
        public string Name { get; set; }
    }
    public class CreateResponse
    {
        public int ResponseCode { get; set; }
        public string ResponsMessage { get; set; }
        public int status { get; set; }
        public CreateMemoModel data { get; set; }
        public HomeViewModel details { get; set; }
        public RequestDetails MemoRequestDetails { get; set; }
        public List<Signatories> SignatoryList { get; set; }
        public List<RequestDetails> RequestList { get; set; }
    }
}