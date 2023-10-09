using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace db_test
{
    /// <summary>
    ///     オペレーター情報クラス
    /// </summary>
    public class Operator
    {
        /// <summary>
        ///     コンストラクタ。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="officeCode"></param>
        public Operator(string code, string name, string officeCode)
        {
            Code = code;
            Name = name;
            OfficeCode = officeCode;
        }

        /// <summary>
        ///     コンストラクタ。
        /// </summary>
        public Operator()
            : this(string.Empty, string.Empty, string.Empty)
        {
        }


        /// <summary>
        ///     オペレーターが選択されているかどうかを取得します。
        /// </summary>
        public bool IsSelected
        {
            get { return !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Name); }
        }

        /// <summary>
        ///     オペレーターコードを取得します。
        /// </summary>
        public string Code
        {
            get;
            protected set;
        }

        /// <summary>
        ///     オペレーター名を取得します。
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <summary>
        ///     事業所コードを取得します。
        /// </summary>
        public string OfficeCode
        {
            get;
            protected set;
        }
    }
}
