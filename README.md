# uwp-shop-analytics-sample
This is the sample code that was demoed in [.NET Universal Windows Platform Development](https://channel9.msdn.com/events/dotnetConf/2016/NET-Universal-Windows-Platform-Development) talk for dotnetConf 2016. 

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

##Getting Started
*More detailed getting start instructions to follow soon*

###Azure Services
In order to be able to execute the code and test the sample fully, you will need to register for the following Azure services and place the appropriate keys/URLs into the keys.resx file in the ShopAnalyticsPCL project.
* Azure App Service - Used to host the ASP.NET Application
* Azure DocumentDB Account - In addition, you will need to create a Database, collection inside the database, and a document within the collection
* Azure Notification Hub and Azure Notification Hub Namespace

###IoT Hardware
For the IoT Client application, I used the following hardware
* Raspberry Pi 2
* [2x 5mm IR Beam Break Sensors](https://www.adafruit.com/products/2168)
* 2x Simple LEDs, one red and one yellow (optional - used as an inidicator to determine when the beam sensor is activated)

##License
This sample is released under the MIT license

##Contribution
This repo is currently not open to contributions.
