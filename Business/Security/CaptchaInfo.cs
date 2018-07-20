using System;

namespace FavLink.Business.Security {

    public sealed class CaptchaInfo : TimeBasedCredential {

        private string _value;

        public CaptchaInfo() { }

        public CaptchaInfo(string value) {
            _value = value;
        }

        public string Value {
            get { return _value; }
            set { _value = value; }
        }

        public override int Duration {
            get {
                return 5 * 60; // 验证码要求5分钟内使用才有效
            }
        }

        protected override string[] GetSerializeFields() {
            return new string[] {
                _value
            };
        }

        public override bool Deserialize(string credential) {
            try {
                string[] values = GetSerializeValues(credential);
                //base.MD5 = values[0];
                //base.PublishTime = Convert.ToDateTime(values[1]);
                this.Value = values[VALUE_FIELD_START_INDEX + 0];
                return true;
            } catch(Exception) {
                return false;
            }
        }

        //void Test() {
        //    CaptchaInfo info = new CaptchaInfo("wfyfngu");
        //    string cre = info.Serialize();
        //    Console.WriteLine(cre);

        //    CaptchaInfo info2 = new CaptchaInfo();
        //    if (info2.Deserialize(cre)) {
        //        Console.WriteLine(info2.IsInvalidOrOverdue);
        //        Console.WriteLine(info2.PublishTime);
        //        Console.WriteLine(info2.Value);
        //    } else {
        //        Console.WriteLine("Faild");
        //    }
        //    Console.WriteLine("Wait...");
        //    System.Threading.Thread.Sleep(3 * 1000); // 等待3秒
        //    Console.WriteLine("Valid:{0}",info2.IsValid);
        //    Console.WriteLine("Due:{0}",info.IsDue);
        //    Console.WriteLine(info2.IsInvalidOrOverdue);
        //}

    }
}
