using System;
using Sail.Common;

namespace Lamtip.Model
{
    [HTable]
    public class Sequence : IModel
    {
        [HColumn(IsPrimary = true, IsIdentity = true)]
        public int SequenceId { set; get; }


        [HColumn(Length = 200)]
        public string SeqName { set; get; }

        [HColumn]
        public int SeqValue { set; get; }

        public int CurrentVal() { return SeqValue; }

        protected int NextVal(IDataContext db)
        {
            SeqValue += 1;
            db.Save(this);
            return SeqValue;
        }

        /// <summary>
        /// 获取下一个流水号,请在事务中使用……
        /// </summary>
        /// <param name="db"></param>
        /// <param name="seqName">流水号名称</param>
        /// <returns></returns>
        public static int NextVal(IDataContext db, string seqName)
        {
            var model = db.GetModel<Sequence>(s => s.SeqName == seqName);
            if (model.IsNull())
                model = new Sequence { SeqName = seqName, SeqValue = 0 };
            return model.NextVal(db);
        }


        public static string NextNumber(IDataContext db, string seqName, int length)
        {
            var val = NextVal(db, seqName);
            return seqName + val.ToString().PadLeft(length, '0');
        }

        /// <summary>
        /// 直接获取下一个流水号
        /// </summary>
        /// <param name="db"></param>
        /// <param name="companyId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string NextNumber(IDataContext db, string code)
        {
            var sn = "{1}{0}".FormatWith(DateTime.Now.ToString("yyMM"), code);
            return sn + Sequence.NextVal(db, sn).ToString().PadLeft(4, '0');
        }
    }

    
}




