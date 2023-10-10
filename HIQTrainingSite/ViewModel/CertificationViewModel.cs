using HIQTraining.ModelDetail;
using HIQTraining.Utils;
using HIQTrainingSite.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HIQTrainingSite.ViewModel {
    public class CertificationViewModel : AlertViewModel {

        public CertificationViewModel() {
            EntitiesList = new List<FormativeEntityDetail>();
            CertificationList = new List<CertificationTypeDetail>();
            Date = DateTime.Today;
        }

        public int Id { get; set; }

        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        [Display(Name = "labelCertificationName", ResourceType = typeof(HIQResources))]
        public string Name { get; set; }
  

        public IEnumerable<SelectListItem> CertificationDrop
        {
            get
            {
                return new SelectList(CertificationList, "Id", "Name", Name);
            }
        }

        public IList<CertificationTypeDetail> CertificationList { get; set; }

        public int CertificationTypeId { get; set; }

        [Display(Name = "labelCertificationName", ResourceType = typeof(HIQResources))]
        public string CertificationType { get; set; }

        [Display(Name = "labelClassification", ResourceType = typeof(HIQResources))]
        [Range(0.0, 100.0)]
        public decimal Result { get; set; }

        [Display(Name = "labelCertificationStatus", ResourceType = typeof(HIQResources))]
        public int? StatusId { get; set; }

        [Display(Name = "labelCertificationStatus", ResourceType = typeof(HIQResources))]
        public string StatusDescription {
            get {
                return (StatusId == null) ? null : CertificationStatus.GetStatusDescription(StatusId.Value);
            }
        }

        public IEnumerable<SelectListItem> StatusDrop {
            get {
                return new SelectList(CertificationStatus.GetStates(), "Id", "Description", StatusId);
            }
        }

        [Display(Name = "labelFormativeEntityName", ResourceType = typeof(HIQResources))]
        public int SelectedEntityId { get; set; }

        [Display(Name = "labelFormativeEntityName", ResourceType = typeof(HIQResources))]
        public string SelectedEntity { get; set; }

        public IList<FormativeEntityDetail> EntitiesList { get; set; }

        public IEnumerable<SelectListItem> EntitiesDrop {
            get {
                return new SelectList(EntitiesList, "Id", "Name", SelectedEntityId);
            }
        }

        [Display(Name = "labelCode", ResourceType = typeof(HIQResources))]
        [MaxLength(20)]
        [StringLength(20, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        //[CodeValidation("Code")]
        public string Code { get; set; }

        [Display(Name = "labelStudanteNameFull", ResourceType = typeof(HIQResources))]
        [MaxLength(50)]
        [StringLength(50, ErrorMessageResourceName = "errorMessageStringMaxMinLength", ErrorMessageResourceType = typeof(HIQResources), MinimumLength = 3)]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public string StudentName { get; set; }

        public int StudentId { get; set; }

        [Display(Name = "labelEmail", ResourceType = typeof(HIQResources))]
        public string StudentEmail { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "labelDate", ResourceType = typeof(HIQResources))]
        [Required(ErrorMessageResourceName = "errorMessageRequired", ErrorMessageResourceType = typeof(HIQResources))]
        public DateTime Date { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "labelObservations", ResourceType = typeof(HIQResources))]
        public string Observation { get; set; }

        [Display(Name = "labelpdf", ResourceType = typeof(HIQResources))]
        public string pdf { get; set; }

    }

}

