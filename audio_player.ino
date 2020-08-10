#include "SD.h"
#include "TMRpcm.h"
#include "SPI.h"

TMRpcm music;
boolean debounce1=true;
boolean debounce2=true;
int song_number=0;
const int CS = 10;
byte address= 0x00;
int level = 1;
char input;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.println("Initializing SD card...");
  if (!SD.begin(4)){
    Serial.println("SD fail");
    return;
  }
  Serial.println("Success");

  music.speakerPin = 9;
 // music.setVolume(5);
  music.quality(1);

  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);

  pinMode(CS, OUTPUT);
  SPI.begin();

}


void loop() {
  
   if (digitalRead(2) == LOW && debounce1==true){
    song_number++;
    if (song_number==3){
      song_number=0;
    }
    debounce1=false;
    Serial.println("Button 1 pressed!");
    Serial.print("Song number: ");
    Serial.println(song_number);

    if(song_number==0){
      music.play("one.wav");
    }
    if(song_number==1){
      music.play("two.wav");
    }
    if(song_number==2){
      music.pause();
    }
    
  }

  if (digitalRead(3)==LOW && debounce2==true){
    Serial.println("Button 2 is pressed!");
    //music.setVolume(level);
    //level++;
    for (int level = 128; level >= 0; level--){
      digitalpot_write(level);
      Serial.print(".");
      delay(100);
    }
   // digitalpot_write(10);
    debounce2=false;
  }

  if(digitalRead(2)==HIGH){
    debounce1=true;
  }
  if(digitalRead(3)==HIGH){
    debounce2=true;
  }

  if (Serial.available()){
    input = Serial.read();
    if(input == '1'){
      music.play("one.wav");
    }
    if(input == '2'){
      music.play("two.wav");
    }
  }
}

void digitalpot_write(int value){
  digitalWrite(CS, LOW); //select chip
  SPI.transfer(address);
  SPI.transfer(value);
  digitalWrite(CS, HIGH); //deselect chip
}
