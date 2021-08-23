using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExam
{/// <summary>
/// read an d write Fields and properties 
/// </summary>
    class Customer
    {
        #region FIELDS AND PROPERTIES 
        public int ID { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public int Height { get; set; }

        #endregion

        /// <summary>
      /// 
      /// </summary>

        #region CONSTRUCTOR
        public Customer() { }
        #endregion

        /// <summary>
        /// method will calculate the bmi of the customer. 
        /// </summary>
        /// <returns>BMI </returns>
        #region METHODS
        public double CalculateBMI() {
            double meter = this.Height *0.01;
             return (this.Weight )/ (meter*meter);
          
        }
        #endregion

    }
}
