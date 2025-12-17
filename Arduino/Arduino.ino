#include <IRremote.hpp>

const byte IR_RECEIVE_PIN = 2;  // Receive IR Data Out on Pin 2

void setup() {
  Serial.begin(9600);
  
  // Activate the IR Receiver
  IrReceiver.begin(IR_RECEIVE_PIN, ENABLE_LED_FEEDBACK);
}

void loop() {
  // Check to see if an IR code is received
  if (IrReceiver.decode()) {
    
    // Check if the signal is not a repeat or error
    if (IrReceiver.decodedIRData.command != 0) 
    {
      // Map the hex values
      switch (IrReceiver.decodedIRData.command) {
        case 0x45: Serial.println("You press the Power button"); break;
        case 0x46: Serial.println("You press the VOL+ button"); break;
        case 0x47: Serial.println("You press the FUNC/STOP button"); break;
        case 0x44: Serial.println("You press the Rewind button"); break;
        case 0x40: Serial.println("You press the Play/Pause button"); break;
        case 0x43: Serial.println("You press the Fast Forward button"); break;
        case 0x07: Serial.println("You press the Arrow Down button"); break;
        case 0x15: Serial.println("You press the VOL- button"); break;
        case 0x09: Serial.println("You press the Arrow Up button"); break;
        case 0x19: Serial.println("You press the EQ button"); break;
        case 0x0D: Serial.println("You press the ST/REPT button"); break;
        case 0x16: Serial.println("You press the 0 button"); break;
        case 0x0C: Serial.println("You press the 1 button"); break;
        case 0x18: Serial.println("You press the 2 button"); break;
        case 0x5E: Serial.println("You press the 3 button"); break;
        case 0x08: Serial.println("You press the 4 button"); break;
        case 0x1C: Serial.println("You press the 5 button"); break;
        case 0x5A: Serial.println("You press the 6 button"); break;
        case 0x42: Serial.println("You press the 7 button"); break;
        case 0x52: Serial.println("You press the 8 button"); break;
        case 0x4A: Serial.println("You press the 9 button"); break;
        
        default:
          Serial.print("Unknown button pressed. Command: 0x");
          Serial.println(IrReceiver.decodedIRData.command, HEX);
          break;
      }
    }
    
    // Tell the IR Receiver to listen for the next code
    IrReceiver.resume();
  }
}