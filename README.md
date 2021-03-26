# AchSmartHome
My smart home on Raspberry Pi and Arduino. Abbreviated as SH or ASH.

## Components
|Device|Filename|Description|Programming Language|Requires|
|:----:|:------:|:---------:|:------------------:|:------:|
|Raspberry Pi|`rsh.cpp`|Receiver (**R**eceiving **S**mart **H**ome program)|C++|[WiringPi](http://wiringpi.com/download-and-install/), Library [RF24](https://github.com/nRF24/RF24)|
|Raspberry Pi|[speaking_clock](https://github.com/DarkCat09/AchSmartHome/tree/master/Raspberry/speaking_clock)|Smart speaking clock (not necessary to use in your SH)|Python|Python3, [SpeechRecognition](https://pypi.org/project/SpeechRecognition/), [gTTS](https://pypi.org/project/gTTS/), [pygame](https://pypi.org/project/pygame/), [requests](https://pypi.org/project/requests/), [Wikipedia](https://pypi.org/project/wikipedia/)|
|Arduino|`doorbell.ino`|Smart doorbell. Taking three photos on click|Wiring C++|Libraries [ArduCAM](https://github.com/ArduCAM/Arduino) and [RF24](https://github.com/nRF24/RF24)|
|Arduino|`temp.ino`|Temperature sensor|Wiring C++|Library [RF24](https://github.com/nRF24/RF24)|
|Arduino|`watering.ino`|Auto-watering plants|Wiring C++|Library [RF24](https://github.com/nRF24/RF24)|
|Your computer|[AchSmartHome_Management](https://github.com/DarkCat09/AchSmartHome/tree/master/AchSmartHome_Management)|Windows application for monitoring sensors|C#|.NET Framework 4.7.2|
|Your android phone|AchSmartHome|Mobile application for monitoring sensors|Java|Android 4.1+|

## Buying
I bought [Raspberry Pi 4B](https://amperka.ru/product/malina-v4-2gb), Iskra boards [Neo](https://amperka.ru/product/iskra-neo) (analog to Arduino Leonardo) and [Nano Pro](https://amperka.ru/product/iskra-nano-pro) (improved Arduino Nano) in the Russian online store **Amperka.Ru**.
You can buy OV2640 for a smart doorbell on AliExpress: https://aliexpress.ru/item/33046344720.html

## Raspberry Pi
Pinout: https://pinout.xyz/pinout/wiringpi
SPI: https://www.raspberrypi.org/documentation/hardware/raspberrypi/spi/README.md#hardware
