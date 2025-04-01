using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB_Browser.ViewModels
{
    public interface ISearchable
    {
        string SearchQuery { get; set; }
    }
}
