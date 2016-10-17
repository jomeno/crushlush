using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crushlush.Core.Business
{
    //  Model and all children are simply data transformation objects (DTO)
    public class Model
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; }

        public Model()
        {
            CreatedAt = DateTime.Now;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void Assign(object source)
        {
            if (source != null)
            {
                var destinationProperties = this.GetType().GetProperties();
                var sourceProperties = source.GetType().GetProperties();

                foreach (var sourceProperty in sourceProperties)
                {
                    foreach (var destProperty in destinationProperties)
                    {
                        if (destProperty.Name == sourceProperty.Name && destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                        {
                            destProperty.SetValue(this, sourceProperty.GetValue(source, new object[] { }), new object[] { });
                            break;
                        }
                    }
                }
            }
        }
    }
}
