//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoShoppingWebside.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class table_Member
    {
        
        public int Id { get; set; }
        [Required]
        [DisplayName("帳號")]
        public string UserId { get; set; }
        [Required]
        [DisplayName("密碼")]
        public string Password { get; set; }
        [Required]
        [DisplayName("姓名")]
        public string Name { get; set; }
        [Required]
        [DisplayName("電子郵件")]
        public string Email { get; set; }
    }
}
