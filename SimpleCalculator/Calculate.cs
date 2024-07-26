using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    //internal class Calculate  // internal 접근 제한자
                                // internal : 동일 어셈블리 내에서만 접근이 가능
                                // 이 Calculate 클래스는 동일한 프로젝트 내에서는 접근할 수 있지만
                                // 다른 프로젝트에서는 접근할 수 없음
    //{
    //}

    public static class Calculate   // public: 모든 어셈블리에서 접근 가능
                                    // static 클래스는 인스턴스를 생성할 수 없고
                                    // 클래스 내의 모든 멤버도 static 이어야 한다.
    {
        public static double DoCalculation(double val1, double val2, string operatiorMath)
        {
            double result = 0;
            switch(operatiorMath)
            {
                case "/":
                    result = val1 / val2;
                    break;
                case "x":
                    result = val1 * val2;
                    break;
                case "-":
                    result = val1 - val2;
                    break;
                case "+":
                    result = val1 + val2;
                    break;
            }
            return result;
        }
    }
}
