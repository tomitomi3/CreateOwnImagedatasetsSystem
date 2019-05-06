//--------------------------------------------------------------------
//フルカラーシリアルLEDモジュール
//↓モノ
//https://www.switch-science.com/catalog/1398/
//--------------------------------------------------------------------
//https://github.com/adafruit/Adafruit_NeoPixel
#include "Adafruit_NeoPixel.h"

//config and vars
#define PIN       9
#define NUMPIXELS 1
Adafruit_NeoPixel pixels(NUMPIXELS, PIN, NEO_GRB + NEO_KHZ800);

void setup() {
  // put your setup code here, to run once:
  pixels.begin();
}

void loop() {
  //all off
  pixels.clear();

  //loop
  while (1)
  {
    for (int i = 0; i < 256; i++)
    {
      //color
      pixels.setPixelColor(0, pixels.Color(i, i, i));
      pixels.show();
      delay(50);
      
      //color
      pixels.setPixelColor(0, pixels.Color(i, i, i));
      pixels.show();
      delay(50);
    }
  }
}
