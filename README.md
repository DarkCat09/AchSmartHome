# AchSmartHome
My smart home on Raspberry Pi and Arduino. Abbreviated as SH or ASH.

## Components
|Device|Filename|Description|Programming Language|Requires|
|:----:|:------:|:---------:|:------------------:|:------:|
|Raspberry Pi|`rsh.out`|Receiver (**R**eceiving **S**mart **H**ome program)|C++|WiringPi, Library nRF|
|Raspberry Pi|[`speaking_clock.py`](https://github.com/DarkCat09/AchSmartHome/blob/master/Raspberry/speaking_clock.py)|Smart speaking clock (not necessary to use in your SH)|Python|Python3, [SpeechRecognition](https://pypi.org/project/SpeechRecognition/), [gTTS](https://pypi.org/project/gTTS/), [pygame](https://pypi.org/project/pygame/)|
|Arduino|`doorbell.ino`|Smart doorbell. Taking three photos on click|Wiring C++|Libraries ArduCAM and nRF|
|Arduino|`temp.ino`|Temperature sensor|Wiring C++|Library nRF|
|Arduino|`watering.ino`|Auto-watering plants|Wiring C++|Library nRF|
|Your computer|[AchSmartHome_Management](https://github.com/DarkCat09/AchSmartHome/tree/master/AchSmartHome_Management)|Windows application for monitoring sensors|C#|.NET Framework 4.7.2|
|Your android phone|AchSmartHome|Mobile application for monitoring sensors|Java|Android 4.1+|

## Buying
I bought [Raspberry Pi 4B](https://amperka.ru/product/malina-v4), Iskra boards [Neo](https://amperka.ru/product/iskra-neo) (analog to Arduino Leonardo) and [Nano Pro](https://amperka.ru/product/iskra-nano-pro) (improved Arduino Nano) in the Russian online store **Amperka.Ru**.
You can buy OV2640 for a smart doorbell on AliExpress: https://aliexpress.ru/item/33046344720.html
