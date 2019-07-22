//--------------------------------------------------------------------
//NeoPixel ctrl
//https://www.switch-science.com/catalog/1398/
//https://github.com/adafruit/Adafruit_NeoPixel
//--------------------------------------------------------------------
#include "Adafruit_NeoPixel.h"

//config and vars
#define PIN       9
#define NUMPIXELS 2
Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

//232C control
#define BAUDRATE 9600

//func
void Clear();
void DebugBlink(unsigned int num);

//vars
#define RCV_SIZE (3+NUMPIXELS*4) //CheckSum:2byte BrigthNess:1byte LEDCtrl:4byte
byte    rcvData[RCV_SIZE];
#define RCV_LOOP (RCV_SIZE*1000)/(BAUDRATE/8)+10 //RCV_SIZE 受信するのにかかる時間

//protocol
//[CHECKSUM1][CHECKSUM2][BRIGHTNESS][CH1][CH1R][CH1G][CH1B][CH2][CH2R][CH2G][CH2B]...

//--------------------------------------------------------------------
//Setup()
//--------------------------------------------------------------------
void setup() {
  //init serial
  Serial.begin(BAUDRATE);

  // put your setup code here, to run once:
  pixels.begin();

  //debug
  pinMode(LED_BUILTIN, OUTPUT);
}

//--------------------------------------------------------------------
//loop()
//--------------------------------------------------------------------
void loop() {
  //all off
  pixels.clear();

  //serial
  while (1)
  {
    //[SERIAL_BUFFER_SIZE]の半分くらいをためてまとめてreadしたほうが早いと思うが逐次読み込む方で
    if (Serial.available() > 0)
    {
      //data clear
      Clear();

      //最大RCV_SIZE分読み込む
      unsigned int chkSum = 0;
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

            //先頭2バイトはチェックサム
            if (readCount >= 2)
            {
              chkSum += temp;
            }

            readCount++;
            if (readCount == RCV_SIZE)
            {
              break;
            }
          }
        }
      }

      //debug
      /*
        delay(500);
        DebugBlink(rcvSumSize);
        delay(500);
        DebugBlink(chkSum);
        delay(1000);
      */

      //checksumを比較して正しければLED制御
      bool isOK = true;
      unsigned int rcvSumSize = (rcvData[1] << 8) | rcvData[0];
      if ( rcvSumSize == chkSum)
      {
        //debug
        DebugBlink(1);

        //to NeoPixel
        pixels.setBrightness(rcvData[2]);
                
        int chIndex = 3;
        for ( int i = 0; i < NUMPIXELS; i++)
        {
          pixels.setPixelColor(rcvData[chIndex], pixels.Color(rcvData[chIndex + 1], rcvData[chIndex + 2], rcvData[chIndex + 3])); //1
          chIndex+=4;
        }
        pixels.show();
      }
      else
      {
        isOK = false;
      }

      //プログラム側でchecksum結果を確認する場合
      /*
        Serial.write(isOK);
        Serial.flush();
      */
    }
  }
}

void Clear()
{
  for (int i = 0; i < RCV_SIZE; i++)
  {
    rcvData[i] = 0;
  }
}

void DebugBlink(unsigned int num)
{
  for (int i = 0; i < num; i++)
  {
    digitalWrite(LED_BUILTIN, HIGH);
    delay(50);
    digitalWrite(LED_BUILTIN, LOW);
    delay(50);
  }
}
