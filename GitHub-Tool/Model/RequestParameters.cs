using System.Reflection;

namespace GitHubSearch.Model
{
    public class RequestParameters
    {

        public override string ToString()
        {
            string requestParametersToString = string.Empty;

            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                requestParametersToString += propertyInfo.Name + "\t" + propertyInfo.GetValue(this) + "\r\n";
            }

            return requestParametersToString;
        }

    }
}
