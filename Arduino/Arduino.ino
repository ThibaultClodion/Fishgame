#include <IRremote.hpp>
#include <LiquidCrystal.h>

const byte IR_RECEIVE_PIN = 13;  // Receive IR Data Out on Pin 2
const int rs = 12, en = 11, d4 = 5, d5 = 4, d6 = 3, d7 = 2;
LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

// Structure pour l'envoi binaire à Unity
struct __attribute__((__packed__)) Message {
    char type;
    uint16_t value;
};

void setup() {
  Serial.begin(9600);
  
  // Activate the IR Receiver
  IrReceiver.begin(IR_RECEIVE_PIN, ENABLE_LED_FEEDBACK);

  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
}

void loop() {
  // Check to see if an IR code is received
  if (IrReceiver.decode()) {
    
    // Check if the signal is not a repeat or error
    if (IrReceiver.decodedIRData.command != 0) 
    {
      Message msg;
      msg.type = 'I'; // 'I' pour IR Remote
      msg.value = (uint16_t)IrReceiver.decodedIRData.command;

      // Envoi du message (3 octets) + un retour à la ligne pour le ReadLine d'Unity
      Serial.write((uint8_t*)&msg, sizeof(msg));
      Serial.print('\n');
    }
    
    // Tell the IR Receiver to listen for the next code
    IrReceiver.resume();
  }
}

void processIncomingMessage(String msg) 
{
  if (msg.startsWith("LCD:")) {
    String data = msg.substring(4); 

    int firstComma = data.indexOf(',');
    int secondComma = data.indexOf(',', firstComma + 1);

    if (firstComma != -1 && secondComma != -1) {
      int column = data.substring(0, firstComma).toInt();
      int line = data.substring(firstComma + 1, secondComma).toInt();
      String textToDisplay = data.substring(secondComma + 1);

      lcd.setCursor(column, line);
      lcd.print(textToDisplay);
    }
  } 
}

// Handles incoming messages
// Called by Arduino if any serial data has been received
void serialEvent()
{
  String message = Serial.readStringUntil('\n');
  processIncomingMessage(message);
}