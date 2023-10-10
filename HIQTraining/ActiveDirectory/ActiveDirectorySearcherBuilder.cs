using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIQTraining.ActiveDirectory
{
    public class ActiveDirectorySearcherBuilder
    {
        public DirectorySearcher Searcher { get; }

        public ActiveDirectorySearcherBuilder(string connectionString, string user, string password)
        {
            var path = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;

            DirectoryEntry connection = new DirectoryEntry(path, user, password);
            Searcher = new DirectorySearcher(connection);
        }

        public ActiveDirectorySearcherBuilder(string connectionString)
        {
            var path = ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;

            DirectoryEntry connection = new DirectoryEntry(path);
            Searcher = new DirectorySearcher(connection);
        }


        public ActiveDirectorySearcherBuilder AddPropertiesToLoad(string name)
        {
            Searcher.PropertiesToLoad.Add(name);
            return this;
        }

        public ActiveDirectorySearcherBuilder AddFilter(string filter, FilterType filterType)
        {
            if (filterType == FilterType.UserName)
            {
                Searcher.Filter = "(&(objectCategory=Person)(objectClass=user)(lastlogon=*)(cn=" + filter + "*))";
            }
            if (filterType == FilterType.Email)
            {
                Searcher.Filter = "(&(objectCategory=Person)(objectClass=user)(lastlogon=*)(mail=" + filter + "))";
            }
            return this;
        }


        public DirectorySearcher Build()
        {
            return Searcher;
        }
    }
}
