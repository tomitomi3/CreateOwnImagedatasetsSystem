//--------------------------------------------------------------------
//NeoPixel ctrl
//https://www.switch-science.com/catalog/1398/
//https://github.com/adafruit/Adafruit_NeoPixel
//--------------------------------------------------------------------
#include "Adafruit_NeoPixel.h"

//config and vars
#define PIN       9
#define NUMPIXELS 5
Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

//UART 232C control
#define BAUDRATE 9600 //9600 115200

//func
void Clear();
void DebugBlink(unsigned int num);

//vars
#define RCV_SIZE (3+NUMPIXELS*4) //CheckSum:2byte BrigthNess:1byte LEDCtrl:4byte
byte    rcvData[RCV_SIZE];
#define RCV_LOOP (RCV_SIZE*1000)/(BAUDRATE/8)*1.5 //RCV_SIZE 受信するのにかかる時間
bool    oneShot = false;

//Protocol
// [CHECKSUM1][CHECKSUM2][BRIGHTNESS][CH1][CH1R][CH1G][CH1B][CH2][CH2R][CH2G][CH2B]...
// [0]        [1]        [2]         [3]  [4]   [5]   [6]   [CH_START*2]...
#define CH_START 3

//--------------------------------------------------------------------
//Setup()
//--------------------------------------------------------------------
void setup() {
  //init serial
  Serial.begin(BAUDRATE);

  // put your setup code here, to run once:
  pixels.begin();

  //debug
  pinMode(LED_BUILTIN, OUTPUT); //for arudino UNO
  pinMode(LED_BUILTIN, PIN);
}

//--------------------------------------------------------------------
//loop()
//--------------------------------------------------------------------
void ResetPixel()
{
  pixels.clear();
  for ( int i = 0; i < NUMPIXELS; i++)
  {
    pixels.setPixelColor(i, pixels.Color(0, 0, 0));
  }
  pixels.show();

}

void HelloNeopixel()
{
  for ( int i = 0; i < NUMPIXELS; i++)
  {
    pixels.setPixelColor(i, 128, 0, 0);
    pixels.show();
    delay(100);
    pixels.setPixelColor(i, 0, 128, 0);
    pixels.show();
    delay(100);
    pixels.setPixelColor(i, 0, 0, 128);
    pixels.show();
    delay(100);
    pixels.setPixelColor(i, 128, 128, 128);
    pixels.show();
    delay(100);
  }
  delay(500);
  ResetPixel();
}

void loop() {
  ResetPixel();
  HelloNeopixel();

  //serial
  while (1)
  {
    //[SERIAL_BUFFER_SIZE]の半分くらいをためてまとめてreadしたほうが早いと思うが逐次読み込む方で
    if (Serial.available() > 0)
    {
      //data clear
      memset(rcvData, 0, RCV_SIZE);

      //最大RCV_SIZE分読み込む
      unsigned int chkSumRcv = 0;
      int readCount = 0;
      for (int i = 0; i < RCV_LOOP; i++)
      {
        //wait
        delay(1);

        //read
        unsigned int canReadSize = Serial.available();
        if (canReadSize > 0)
        {
          for (int i = 0; i < canReadSize; i++)
          {
            byte temp  = Serial.read();
            rcvData[readCount] = temp;

            //受信先頭2バイトはチェックサム
            if (readCount >= 2)
            {
              chkSumRcv += temp;
            }

            //RCV_SIZE分
            readCount++;
            if (readCount == RCV_SIZE)
            {
              break;
            }
          }
        }
      }

      //checksum確認 加算して0であればOK
      chkSumRcv +=  (rcvData[1] << 8) | rcvData[0];
      if ( chkSumRcv == 0)
      {
        //to NeoPixel
        pixels.setBrightness(rcvData[2]);
        
        //ctrl
        int LEDNUM = (readCount - 3) / 4; //7byte = 2byte:checksum 1byte:brightness CH0LED
        int index = CH_START;
        for ( int i = 0; i < LEDNUM; i++)
        {
          pixels.setPixelColor(rcvData[index], pixels.Color(rcvData[index + 1], rcvData[index + 2], rcvData[index + 3]));
          index += 4;
        }
        pixels.show();

        //プログラム側でchecksum結果を確認する場合
        //Serial.write(1);
      }
      else
      {
        //プログラム側でchecksum結果を確認する場合
        //Serial.write(0);
      }
    }
  }
}

void DebugBlink(int pin, unsigned int num)
{
  for (int i = 0; i < num; i++)
  {
    digitalWrite(pin, HIGH);
    delay(50);
    digitalWrite(pin, LOW);
    delay(50);
  }
}
