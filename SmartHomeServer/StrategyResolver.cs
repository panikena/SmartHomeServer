﻿using SmartHomeServer.Enums;
using SmartHomeServer.Messages;
using SmartHomeServer.ProcessingModules;
using SmartHomeServer.ProcessingModules.SystemSideModules;
using SmartHomeServer.ProcessingModules.UserSideModules;
using System;

namespace SmartHomeServer
{
    public class ProcessingStrategyResolver
    {
        public ProcessingStrategyResolver()
        {

        }

        public IModule GetProcessingModule(IMessage message)
        {
            if (message.Source == MessageSource.WebSocket)
            {
                WebSocketMessage webSocketMessage = (WebSocketMessage)message;
                if (webSocketMessage.WidgetType == WidgetType.ColorPickerSwitch)
                {
                    return new ColorPickerSwitchModule();
                }
                if (webSocketMessage.WidgetType == WidgetType.LightSwitch)
                {
                    return new LightSwitchModule();
                }
                return new EchoModule();
            }
            if (message.Source == MessageSource.UnixSocket)
            {
                SmartBrickMessage smartBrickMessage = (SmartBrickMessage)message;
                if (smartBrickMessage.SmartBrickID == 25)
                {
                    return new ThermoModule();
                }
                return new TestUnixModule();
            }
            throw new Exception("No module found");
        }
    }
}
