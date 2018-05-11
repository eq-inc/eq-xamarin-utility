using System;
using System.Diagnostics;
using System.Text;

namespace Eq.Utility
{
    public class LogController
    {
        public const System.Int64 LogCategoryMethodIn = 0x0000000000000001 << 1;
        public const System.Int64 LogCategoryMethodTrace = 0x0000000000000001 << 2;
        public const System.Int64 LogCategoryMethodOut = 0x0000000000000001 << 3;
        public const System.Int64 LogCategoryMethodInfo = 0x0000000000000001 << 61;
        public const System.Int64 LogCategoryMethodWarn = 0x0000000000000001 << 62;
        public const System.Int64 LogCategoryMethodError = 0x0000000000000001 << 63;
        public const System.Int64 LogCategoryAll = 0x7FFFFFFFFFFFFFFF;
        public const System.Int64 LogCategoryNone = 0;

        private static readonly LogController sLogController = new LogController();

        public static System.Int64 GlobalOutputLogCategory
        {
            get
            {
                return sLogController.mOutputLogCategories;
            }
            set
            {
                sLogController.mOutputLogCategories = value;
            }
        }

        public static string GlobalLogTag
        {
            get
            {
                return sLogController.mLogTag;
            }
            set
            {
                sLogController.mLogTag = value;
            }
        }

        public static void Log(System.Int64 category, params object[] contents)
        {
            sLogController.InnerCategoryLog(category, 1, contents);
        }

        private System.Int64 mOutputLogCategories = LogCategoryNone;
        private string mLogTag = "";
        private int mDefaultIndent = 0;

        public LogController()
        {
            // 未指定の場合はグローバル設定を有効
            if(sLogController != null)
            {
                mOutputLogCategories = sLogController.mOutputLogCategories;
                mLogTag = sLogController.mLogTag;
            }
        }

        public LogController(LogController srcLogController)
        {
            mOutputLogCategories = srcLogController.mOutputLogCategories;
            mLogTag = srcLogController.mLogTag;
        }

        public LogController(System.Int64 outputLogCategories)
        {
            mOutputLogCategories = outputLogCategories;
        }

        public void SetLogTag(string logTag)
        {
            mLogTag = logTag;
        }

        public void ChangeDefaultIndent(int indent)
        {
            mDefaultIndent = indent;
        }

        public void CopyFrom(LogController copyFrom)
        {
            mOutputLogCategories = copyFrom.mOutputLogCategories;
            mLogTag = copyFrom.mLogTag;
        }

        public void CopyTo(LogController copyTo)
        {
            copyTo.CopyFrom(this);
        }

        public void AppendOutputLogCategory(System.Int64 outputLogCategories)
        {
            mOutputLogCategories |= outputLogCategories;
        }

        public void RemoveOutputLogCategory(System.Int64 outputLogCategories)
        {
            mOutputLogCategories &= (~outputLogCategories);
        }

        public void SetOutputLogCategory(System.Int64 outputLogCategories)
        {
            mOutputLogCategories = outputLogCategories;
        }

        public System.Int64 GetOutputLogCategory()
        {
            return mOutputLogCategories;
        }

        public void CategoryLog(System.Int64 category, params object[] contents)
        {
            InnerCategoryLog(category, mDefaultIndent, contents);
        }

        private void InnerCategoryLog(System.Int64 category, int indent, params object[] contents)
        {
            if (category == LogCategoryMethodInfo || category == LogCategoryMethodWarn || category == LogCategoryMethodError)
            {
                StringBuilder contentBuilder = new StringBuilder();
                StackFrame lastStackFrame = new StackTrace(true).GetFrame(indent + 1);

                if (category == LogCategoryMethodInfo)
                {
                    contentBuilder.Append("#INFO#");
                }
                else if (category == LogCategoryMethodWarn)
                {
                    contentBuilder.Append("#WARN#");
                }
                else if (category == LogCategoryMethodError)
                {
                    contentBuilder.Append("#ERROR#");
                }

                if (!string.IsNullOrEmpty(mLogTag))
                {
                    contentBuilder.Append(mLogTag).Append(": ");
                }

                contentBuilder
                    .Append(lastStackFrame.GetMethod())
                    .Append("(")
                    .Append(System.IO.Path.GetFileName(lastStackFrame.GetFileName()))
                    .Append(":")
                    .Append(lastStackFrame.GetFileLineNumber())
                    .Append(")");
                if (contents != null && contents.Length > 0)
                {
                    contentBuilder.Append(": ");
                    foreach (object content in contents)
                    {
                        contentBuilder.Append(content);
                    }
                }

                Console.WriteLine(contentBuilder.ToString());
            }
            else if ((mOutputLogCategories & category) == category)
            {
                switch (category)
                {
                    case LogCategoryMethodIn:
                    case LogCategoryMethodOut:
                        {
                            StringBuilder contentBuilder = new StringBuilder();
                            StackFrame lastStackFrame = new StackTrace(true).GetFrame(indent + 1);
                            if (!string.IsNullOrEmpty(mLogTag))
                            {
                                contentBuilder.Append(mLogTag).Append(": ");
                            }
                            contentBuilder
                                .Append(lastStackFrame.GetMethod());
                            if (!string.IsNullOrEmpty(lastStackFrame.GetFileName()))
                            {
                                contentBuilder.Append("(")
                                                .Append(lastStackFrame.GetFileName())
                                                .Append(":")
                                                .Append(lastStackFrame.GetFileLineNumber())
                                                .Append(")");
                            }
                            contentBuilder.Append((category == LogCategoryMethodIn) ? "(IN)" : "(OUT)");

                            if (contents != null && contents.Length > 0)
                            {
                                contentBuilder.Append(": ");
                                foreach (object content in contents)
                                {
                                    contentBuilder.Append(content);
                                }
                            }
                            Console.WriteLine(contentBuilder.ToString());
                        }
                        break;
                    case LogCategoryMethodTrace:
                        {
                            StringBuilder contentBuilder = new StringBuilder();
                            StackFrame lastStackFrame = new StackTrace(true).GetFrame(indent + 1);

                            if (!string.IsNullOrEmpty(mLogTag))
                            {
                                contentBuilder.Append(mLogTag).Append(": ");
                            }
                            contentBuilder
                                .Append(lastStackFrame.GetMethod());
                            if (!string.IsNullOrEmpty(lastStackFrame.GetFileName()))
                            {
                                contentBuilder.Append("(")
                                .Append(System.IO.Path.GetFileName(lastStackFrame.GetFileName()))
                                .Append(":")
                                .Append(lastStackFrame.GetFileLineNumber())
                                .Append(")");

                            }

                            if (contents != null && contents.Length > 0)
                            {
                                contentBuilder.Append(": ");
                                foreach (object content in contents)
                                {
                                    contentBuilder.Append(content);
                                }
                            }

                            Console.WriteLine(contentBuilder.ToString());
                        }
                        break;
                    default:
                        if (contents != null && contents.Length > 0)
                        {
                            StringBuilder contentBuilder = new StringBuilder();
                            if (!string.IsNullOrEmpty(mLogTag))
                            {
                                contentBuilder.Append(mLogTag).Append(": ");
                            }
                            foreach (string content in contents)
                            {
                                contentBuilder.Append(content);
                            }
                            Console.WriteLine(contentBuilder.ToString());
                        }
                        break;
                }
            }
        }
    }
}
