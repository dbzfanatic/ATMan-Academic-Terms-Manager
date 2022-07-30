using System;
using System.Collections.Generic;
using System.Text;

namespace C868.Models
{
    public static class CourseStatus
    {
        public enum Status {Waiting, Started, InProgress, Testing, Completed};

        public static string ToString(this Status me)
        {
            switch (me)
            {
                case Status.Waiting:
                    return "Waiting";
                case Status.Started:
                    return "Started";
                case Status.InProgress:
                    return "In-Progress";
                case Status.Testing:
                    return "Testing";
                case Status.Completed:
                    return "Completed";
            }
            return null;
        }
    }
}
