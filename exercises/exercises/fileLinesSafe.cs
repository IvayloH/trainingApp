using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace exercisesProject
{
    class fileLinesSafe
    {
        private string[] fileL;
        public fileLinesSafe()
        {
            //this.fileL = fileL;
        }

        public string[] getFileLines()
        {
            return fileL;
        }

        public void setFileLines(string[] fileL)
        {
            this.fileL = fileL;
        }
    }
}
