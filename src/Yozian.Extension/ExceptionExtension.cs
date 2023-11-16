using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Yozian.Extension
{
    public static class ExceptionExtension
    {
        public static string DumpDetail(this Exception ex, Func<StackFrame, bool> filter = null)
        {
            var sb = new StringBuilder();
            var st = new StackTrace(ex, true);
            var innerException = ex;

            while (innerException != null)
            {
                sb.AppendLine($"ErrorMessage: {innerException.Message}");
                innerException = innerException.InnerException;
            }

            sb.AppendLine("--------StackTrace----------");

            var frames = st.GetFrames().AsEnumerable();

            if (null != filter)
            {
                frames = frames?.Where(filter);
            }

            frames?.Select(
                    frame => new
                    {
                        FileName = frame.GetFileName(),
                        LineNumber = frame.GetFileLineNumber(),
                        ColumnNumber = frame.GetFileColumnNumber(),
                        Method = frame.GetMethod(),
                        Class = frame.GetMethod().DeclaringType
                    }
                )
               .Reverse()
               .ForEach(
                    info =>
                    {
                        sb.AppendLine($"class:{info.Class?.FullName}");
                        sb.AppendLine($"method:{info.Method?.Name}");

                        // only print for our source
                        if (!string.IsNullOrEmpty(info.FileName))
                        {
                            sb.AppendLine($"file:{info.FileName}");
                            sb.AppendLine($"line:{info.LineNumber}");
                            sb.AppendLine($"column:{info.ColumnNumber}");
                            sb.AppendLine("----------Next-----------");
                        }
                    }
                );

            // wait to implement serializer
            // sb.AppendLine("--------Data----------");
            // sb.AppendLine(JsonConvert.SerializeObject(ex.Data));

            return sb.ToString();
        }
    }
}
