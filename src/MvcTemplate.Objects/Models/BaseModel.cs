using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcTemplate.Objects
{
    public abstract class BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int32 Id
        {
            get;
            set;
        }

        public DateTime CreationDate
        {
            get
            {
                if (!IsCreationDateSet)
                    CreationDate = DateTime.Now;

                return InternalCreationDate;
            }
            protected set
            {
                IsCreationDateSet = true;
                InternalCreationDate = value;
            }
        }
        private Boolean IsCreationDateSet
        {
            get;
            set;
        }
        private DateTime InternalCreationDate
        {
            get;
            set;
        }
    }
}
