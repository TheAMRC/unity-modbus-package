# RTFM Table of Contents #

The headings here are given as a guide. Please add and remove as your project suits.

* [Hardware](#hardware)
* [Software Servies and Applications](#software-services-and-applications)
* [User Interface](#user-interface)
  * [State Machine Diagram](#state-machine-diagram)
  * [Key Bindings](#key-bindings)
* [Network](#network)
  * [Topology](#topology)
  * [IP Address Space](#ip-address-space)
  * [WiFi](#wifi-details)
  * [Internet Connection](#internet-connection)
* [Data Schema](#data-schema)
  * [Transmission](#transmission)
  * [Storage](#storage)
* [Security](#security)
  * [Encryption](#encryption)
  * [Authentication](#authentication)
  * [Backup](#backup)
  * [User Data](#user-data)
* [FAQ](#faq)

---

## Hardware ##

| Make & Model | MAC Address(es)| Hostname / IP Address | Login Details & Method | Role and Applications |
| --- | --- | --- | --- | --- |
| Apple iPad | A1-B2-C3-D4-E5-F6 | amrc-ipad1 | 1234 (PIN) | Tablet for GUI |
| Dell Precision | DE-AD-BE-EF-00-00 (eth1) <br/> FF-FF-FF-FF (eth2) | 192.168.1.2 <br/> amrc-dellprec1 | amrcuser:f2050 (Windows Login) | Virtual Machine server (nodejs) |
| Linux VM | AB-DE-F0-12-34-56 | 192.168.1.5 | pi:raspberry (SSH) <br/> ftpuser:ftppass (FTP) |Nodejs VM |
| Bosch Rexroth Nexo | 01-02-03-04-05-06 | 192.168.1.7 | N/A |Smart tool |

[back to top](#RTFM-Table-of-Contents)

<!-- Example of an HTML pagebreak when printing to PDF -->
<div style="page-break-after: always;"></div>

## Software Services and Applications ##

| Name | Role | Host Hardware | Login Details & Method | Notes |
| --- | --- | --- | --- | --- |
| VM Host | VM Host | Dell Precision | N/A | Access via Dell Precision above |
| Nodejs | Webserver | Linux VM | user:password (SSH) | All files kept in /opt/nodejs <br/> Controlled via pm2 |
| Frontend | GUI | iPad | amrc:F2050 (login page) | Big red icon on home screen |
| Smarttool | MQTT wrapper | Linux VM | N/A | All files kept in /opt/mqtt-wrapper <br/> Controlled via pm2 |

[back to top](#RTFM-Table-of-Contents)

## User Interface ##

### State Machine Diagram ###

The following diagram describes how the user can traverse the user interface.
<object type="image/svg+xml" data="diagrams/out/sm.svg"></object>

### Key Bindings ###

| Action name | Long description | Mouse shortcut | Keyboard shortcut | Alt keyboard shortcut | When applicable |
|---|---|---|---|---|---|
| Select | Used to select buttons, buildings | Left mouse | Space | Enter | Keyboard only usable when not typing |

## Network ##

### Topology ###

<object type="image/svg+xml" data="diagrams/out/network/network.svg"></object>

### IP Address Space ###

_For projects on the AMRC network, you can delete this section_

* Network: 192.168.1.xx
* Netmask: 255.255.255.0
* Gateway (router): 192.168.1.1
* DNS: 192.168.1.1
* DHCP Range 192.168.1.2 - 127
* Static IP Range 192.168.1.128 - 254

### WiFi Details ###

* SSID: PrettyFlyForAWiFi
* PSK: password123
* IP Range: As above

### Internet Connection ###

Internet is provided via Dell Precision Server. Eth2 is connected to the AMRC network and all VM's share this adapter. Eth2 is the one marked "NIC2", next to the HDMI port.

[back to top](#RTFM-Table-of-Contents)

## Data Schema ##

### Transmission ###

This section details the MQTT message schema.

```javascript
{
  "laserProjector": {
    "deviceID": "laserProjector1",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "startProjection/stopProjection/calibration",
      "projectionFile": "file name from DB (only needed for projection)",
      "projectionLayer": "name of projection layer (only needed for projection)",
      "calibrationFile": "file name (only needed for calibration)"
    },
    "reponse": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "opticalProjector": {
    "deviceID": "opticalProjector1",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start/stop",
      "projectionFile": "file name from DB"
    },
    "reponse": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "smartCaliper": {
    "deviceID": "smartCaliper1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start/stop (enables messages to be sent on button push)"
    },
    "reponse": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code",
      "value": 45
    }
  },
  "pickByLight": {
    "deviceID": "pickByLight1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start/stop",
      "boxPosition": {
        "x0": 0,
        "x1": 20,
        "y0": 0,
        "y1": 30,
        "stripNumber": 1
      }
    },
    "reponse": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "cobot": {
    "deviceID": "cobot1",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start",
      "operationNo": 1,
      "benchHeight": 123.45
    },
    "reponse": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "smartCamera": {
    "deviceID": "smartCamera1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start",
      "inspectionName": "inspection file name"
    },
    "response": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "benchHeight": {
    "deviceID": "benchHeight1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start",
      "benchHeight": 123.45
    },
    "response": {
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "benchLight": {
    "deviceID": "benchLight1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "start",
      "colour": "red/white",
      "intensity": 255
    },
    "response": {
      "status": "OK/In Progress/Finished/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "rfidReader": {
    "deviceID": "rfidReader1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "response": {
      "tagID": "abc123def456",
      "type": "login/logout",
      "status": "OK/Busy/Complete/Error/Pass/Fail",
      "statusText": "any additional status information e.g. error reason or code"
    }
  },
  "smartWatch": {
    "deviceID": "smartWatch1/2",
    "timestamp": "<ISO 8601 Format timestamp>",
    "command": {
      "name": "next/previous/restart",
      "benchId": "bench1/2"
    }
  }
}
```

### Storage ###

Data storage is achieved using mongoDB. Below is the data schemafor data storage.

```javascript
{
  "users": {
    "_id": "abcdef123456",
    "fullName": "John Smith",
    "rfidTag": "abcdef123456",
    "skills": {
      "supervisor": 5,
      "assembly": 4,
      "electrical": 2,
      "drilling": 1,
      "welding": 3
    }
  },
  "benches": {
    "_id": "abcdef1123456",
    "name": "bench1",
    "ipAddress": "192.168.0.10",
    "config": {},
    "currentUser": "John Smith",
    "devices": [
      {
        "id": "rfidReader1",
        "rfidLoginThreshold": -50,
        "loginGraceTime": 5000
      },
      {
        "id": "cobot"
      },
      {
        "id": "laserProjector1"
      },
      {
        "id": "opticalProjector1"
      },
      {
        "id": "smartCamera1"
      },
      {
        "id": "smartCaliper1"
      },
      {
        "id": "benchPLC1"
      },
      {
        "id": "pickByLight1"
      },
      {
        "id": "gluingHead"
      }
    ]
  },
  "products": {
    "_id": "abcdef123462",
    "name": "product1",
    "capableBenches": [
      "bench1",
      "bench2"
    ],
    "productImageUrl": "URL",
    "procedures": [
      {
        "stepName": "StepOne",
        "level1Description": "Some description",
        "level2Description": "Soem further information",
        "videoUrl": "URL",
        "objUrl": "URL",
        "mtlUrl": "URL",
        "stepImageUrl": "URL",
        "equipmentRequired": [
          "safetyGlassesURL",
          "glovesURL",
          "drillURL"
        ],
        "benchDevicesRequired": [
          {
            "id": "cobot",
            "validationRequired": true
          },
          {
            "id": "laserProjector1"
          },
          {
            "id": "benchPLC1",
            "validationRequired": true
          },
          {
            "id": "pickByLight1",
            "validationRequired": true
          },
          {
            "id": "gluingHead",
            "validationRequired": true
          }
        ]
      },
      {
        "stepName": "StepTwo",
        "level1Description": "Some description",
        "level2Description": "Some further information",
        "videoUrl": "URL",
        "objUrl": "URL",
        "mtlUrl": "URL",
        "stepImageUrl": "URL",
        "equipmentRequired": [
          "safetyGlassesURL",
          "glovesURL",
          "drillURL"
        ],
        "benchDevicesRequired": [
          "cobot",
          "laserProjector1",
          "benchPLC1"
        ]
      }
    ]
  },
  "existingProducts": {
    "_id": "abcdef123456",
    "productId": "abcdef123456",
    "status": 1,
    "lastWorkedOnBench": "bench1",
    "lastWorkedOnBy": "John Smith",
    "completedSteps": [
      {
        "dateStarted": "<Javascript Date Object>",
        "lastWorkedOn": "<Javascript Date Object>",
        "dateCompleted": "<Javascript Date Object>",
        "status": 2,
        "user": "<user id>",
        "bench": "<bench id>",
        "validation": {
          "cobot": {
            "status": "ok",
            "timestamp": "<Javascript Date Object>"
          },
          "caliper": {
            "status": "ok",
            "value": 16.5,
            "timestamp": "<Javascript Date Object>"
          }
        }
      },
      {
        "dateStarted": "<Javascript Date Object>",
        "lastWorkedOn": "<Javascript Date Object>",
        "dateCompleted": "<Javascript Date Object>",
        "status": 1,
        "user": "<user id>",
        "bench": "<bench id>",
        "validation": {
          "2ndUser": {
            "status": "ok",
            "value": "<user id>",
            "timestamp": "<Javascript Date Object>"
          }
        }
      }
    ]
  },
  "configs": {
    "statusDefinitions": [
      "Started",
      "Parked",
      "Completed",
      "Overridden/Skipped",
      "WTF'd"
    ],
    "MQTT": {
      "host": "www.example.com",
      "mqttPort": 1883,
      "mqttsPort": 8883,
      "wsPort": 9001,
      "wssPort": 9801,
      "username": "user",
      "password": "password",
      "deviceTypes": [
        "cobot",
        "gluingHead",
        "laserProjector",
        "smartCamera",
        "smartCaliper",
        "benchPLC",
        "rfidReader",
        "pickByLight"
      ]
    }
  }
}
```

[back to top](#RTFM-Table-of-Contents)

## Security ##

A brief descriptive overview of what security protocols are in place where applicable.

### Encryption ###

e.g. SSL encryption is employed on all connections to the server. Include key/certificate here either by pasting into code field or by committing file to this docs folder.

### Authentication ###

e.g. JS web tokens are used to authorize client to server.
Include a table here of any additional login details for systems not documented in hardware or software section.

### Backup ###

Outline any backup strategy employed for data and configuration files (all code should be committed to github).

### User Data ###

In line with GDPR, provide detail in this section of how and where user data is kept and protected.
[back to top](#RTFM-Table-of-Contents)

## FAQ ##

Answer (or at least acknowledge) any common issues or questions.
For example:

* My username and password is being rejected from X software?
    * This often happens when you have a local version installed alongside the container version.
    
[back to top](#RTFM-Table-of-Contents)

### Linked page example ###

[Click here to view the linked page](./linked-page.md)

[back to top](#RTFM-Table-of-Contents)
