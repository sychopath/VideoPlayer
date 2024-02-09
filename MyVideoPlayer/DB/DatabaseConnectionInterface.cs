using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyVideoPlayer.DB
{
    public interface IDatabaseConnection
    {
        public SqlConnection OpenConnection();
        public void CloseConnection();
    }
}
