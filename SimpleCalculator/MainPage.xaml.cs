// this는 현재 객체(일반적으로 클래스의 인스턴스)를 참조
namespace SimpleCalculator
{
    public partial class MainPage : ContentPage
    {
        int currentState = 1;       // 상태 변수, 입력 순서 혹은 입력 모드를 설정하는 듯 한데
                                    // 1: 기본 상태
                                    // -1: 
        string operatorMath;        // 연산자
        double firstNum, secondNum; // 피연산자
        bool isFirstInput = true;

        public MainPage()
        {
            InitializeComponent();
            OnClear(this, null);
        }
        
        // Clear; 초기화
        void OnClear(object sender, EventArgs e)
        {
            firstNum = 0;
            secondNum = 0;
            currentState = 1;
            isFirstInput = true;

            this.CalcText.Text = "0";
            this.result.Text = "0";

        }

        // 제곱 버튼 기능
        void OnSquareRoot(object sender, EventArgs e)
        {
            if(firstNum == 0) { return; }
            firstNum = firstNum * firstNum;
            this.result.Text = firstNum.ToString();
        }

        void OnPercentage(object sender, EventArgs e) { }

        void OnNumberSelection(object sender, EventArgs e)
        {
            //Button button = sender as Button; // 안전한 캐스팅; as 키워드
                                                // sender 객체가 Button 타입이 아닌 경우 예외 발생 대신 null 반환
                                                // 따라서 반환된 객체가 null인지 확인하는 추가 코드가 필요
            Button button = (Button)sender; // 명시적 캐스팅
                                            // sender 객체가 Button 타입이 아닌 경우 InvalidCastException 예외 발생
                                            // 타입이 확실할 때 사용
            string btnPressed = button.Text;    // 버튼을 누르면 해당 숫자 사용

            if (isFirstInput)
            {

                isFirstInput = false;
            }

            if (this.result.Text == "0" || currentState < 0) // 첫 입력 시 (결과 0상태) 혹은 음수 상태일 때 (-2: secondNum 입력순서) 텍스트 비우고 입력
            {
                if (this.result.Text == "0")    // 초기 입력 시에만 계산식 초기화
                {
                    this.CalcText.Text = string.Empty;
                }

                this.result.Text = string.Empty;
                
                if (currentState < 0)   // 음수 상태면 (-2 = 연산자 선택 상태: secondNum 입력 차례라는 뜻; -1 = 계산 완료 상태: 이전 결과가 firstNum이 됨)
                    currentState *= -1; // 양수 상태로 전환: secondNum 입력 순서 (-2 -> 2로 전환) / (-1 -> 1로 전환)
                                        // 2로 전환되어 OnCalculate -> DoCalculation 함수에서 쓰임
            }

            // 버튼 입력값 누적
            this.result.Text += btnPressed;
            this.CalcText.Text += btnPressed;

            double number;
            if (double.TryParse(this.result.Text, out number))  // TryParse(): 문자열을 double 타입의 숫자로 변환
                                                                // 성공 시 true, 실패 시 false
                                                                // 변환이 성공하면 out 매개변수에 저장됨
            {
                // 소숫점 포함 그대로를 표현: 정수부분+소수부분
                // 숫자를 문자열로 변환
                string numberStr = number.ToString();

                // 소수점 위치를 찾아 자릿수 계산; 소숫점이 없으면(-1반환) decimalPlaces = 0;
                int decimalPointIndex = numberStr.IndexOf('.');
                int decimalPlaces = decimalPointIndex == -1 ? 0 : numberStr.Length - decimalPointIndex - 1;

                // 정수 부분과 소수 부분으로 분리
                string[] parts = numberStr.Split('.');                      // 소숫점을 기준으로 분리
                string integerPart = int.Parse(parts[0]).ToString("N0");    // 정수부분 포맷으로 분리
                string decimalPart = parts.Length > 1 ? parts[1] : "";      // 소수부분이 있으면(1=정수만) 소수부분 저장

                // 결합하여 최종 결과 생성
                string formattedNumber = decimalPlaces > 0 ? $"{integerPart}.{decimalPart}" : integerPart;  // 소숫점이 있으면 문자열 보간 사용해 정수+소수 결합, 없으면 정수부분만 취함
                
                this.result.Text = formattedNumber;

                if (currentState == 1)  // 양수 1 상태면 
                {
                    firstNum = number;
                }
                // 다른 양수 혹은 음수 상태면 secondNum 입력 순서
                else
                {
                    secondNum = number;
                }
                
            }
        }

        // 연산자 선택 시 단순히 변수에 연산자 텍스트만 저장하는 함수 -> OnCalculate -> DoCalculation 함수에서 쓰임
        void OnOperatorSelection(object sender, EventArgs e)
        {
            currentState = -2;  // 음수 -2 상태: 연산자 선택 상태
            Button button = (Button)sender;
            string btnPressed = button.Text;
            operatorMath = btnPressed;

            this.CalcText.Text += operatorMath;

        }

        void OnDeleteLastChar(object sender, EventArgs e)
        {
            string newResultText = this.result.Text.Substring(0, this.result.Text.Length - 1);  // 마지막 문자를 제거한 문자열 생성
            this.result.Text = newResultText;

            double number;
            if (double.TryParse(this.result.Text, out number))  // TryParse(): 문자열을 double 타입의 숫자로 변환
                                                                // 성공 시 true, 실패 시 false
                                                                // 변환이 성공하면 out 매개변수에 저장됨
            {
                // result.Text 길이를 조건으로 걸고 그 길이만큼만 calctext, result에서 지울수 있게끔하면 되나?
                if (currentState == 1)  // 양수 1 상태면 
                {
                    firstNum = number;
                }
                // 다른 양수 혹은 음수 상태면 secondNum 입력 순서
                else
                {
                    secondNum = number;
                }
            }
        }

        void OnCalculate(object sender, EventArgs e)
        {
            if (currentState == 2)  // 양수 2 상태: 연산자 선택해 -2 상태에서 [secondNum 입력 이후 상태] *= -1로 2가 됨
            {
                var result = Calculate.DoCalculation(firstNum, secondNum, operatorMath);

                this.result.Text = result.ToString();
                this.CalcText.Text = $"({this.CalcText.Text})";   // 문자열 보간 사용
                // this.CalcText.Text = "(" + this.CalcText.Text + ")";   // 문자열 연결; 이건 안되네

                currentState = -1;  // -1: 계산 완료 상태
            }
        }
    }
}

