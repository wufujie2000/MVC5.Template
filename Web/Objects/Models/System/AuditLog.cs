using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace MvcTemplate.Objects
{
    public class AuditLog : BaseModel
    {
        [Index]
        [StringLength(128)]
        public String AccountId { get; set; }

        [Index]
        [Required]
        [StringLength(128)]
        public String EntityName { get; set; }

        [Index]
        [Required]
        [StringLength(128)]
        public String EntityId { get; set; }

        [Required]
        public String Changes { get; set; }

        public AuditLog()
        {
        }
        public AuditLog(String entityName, String entityId, String changes)
        {
            AccountId = HttpContext.Current.User.Identity.Name;
            EntityName = entityName;
            EntityId = entityId;
            Changes = changes;
        }
    }
}
