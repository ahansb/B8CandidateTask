using System;

namespace Bit8.StudentSystem.Web.Api.Helpers
{
    public class ValidationHelper
    {
        public bool ValidateId(int id)
        {
            if (id <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateRequiredStringProperty(string property)
        {
            if (string.IsNullOrWhiteSpace(property) || property.Length > 45)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateDate(DateTime date)
        {
            if (date == default(DateTime))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateDates(DateTime start, DateTime end)
        {
            if (start == default(DateTime) || end == default(DateTime) || start > end)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateScore(int score)
        {
            if (score < 2 || 6 < score)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateObject(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
