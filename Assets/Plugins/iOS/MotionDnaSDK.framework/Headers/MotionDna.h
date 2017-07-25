#ifndef MOTIONDNA_DATA_H
#define MOTIONDNA_DATA_H

#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>

enum PrimaryMotion
  {
    STATIONARY,
    FIDGETING,
    FORWARD
  };
typedef enum PrimaryMotion PrimaryMotion;

enum SecondaryMotion
  {
    UNDEFINED,
    FORWARD_IN_HAND,
    FORWARD_IN_HAND_SWINGING,
    FORWARD_IN_POCKET,
    FORWARD_IN_CALL,
    DWELLING,
    JUMPING
  };
typedef enum SecondaryMotion SecondaryMotion;

enum PowerConsumptionMode
  {
    SUPER_LOW_CONSUMPTION,
    LOW_CONSUMPTION,
    MEDIUM_CONSUMPTION,
    PERFORMANCE
  };
typedef enum PowerConsumptionMode PowerConsumptionMode;

enum ExternalPositioningState
  {
    OFF,
    HIGH_ACCURACY,
    LOW_ACCURACY,
    EXTERNAL_UNDEFINED
  };
typedef enum ExternalPositioningState ExternalPositioningState;

enum VerticalDirection
  {
    UP,
    DOWN,
    CONSTANT
  };
typedef enum VerticalDirection VerticalDirection;

enum VerticalType
  {
    ESCALATOR_STAIRS,
    ELEVATOR,
    LEVEL
  };
typedef enum VerticalType VerticalType;

enum LocationStatus
  {
    UNINITIALIZED,
    USER_INITIALIZED,
    GPS_INITIALIZED,
    NAVISENS_INITIALIZING,
    NAVISENS_INITIALIZED,
    BEACON_INITIALIZED
  };
typedef enum LocationStatus LocationStatus;

enum CalibrationStatus
  {
    DONE,
    CALIBRATING,
    NONE
  };
typedef enum CalibrationStatus CalibrationStatus;

enum EstimationMode
{
  LOCAL,
  GLOBAL
};
typedef enum EstimationMode EstimationMode;

struct MotionStatistics
{
  double dwelling;
  double walking;
  double stationary;
};
typedef struct MotionStatistics MotionStatistics;

struct VerticalMotionStatus
{
  VerticalDirection direction;
  VerticalType type;
};
typedef struct VerticalMotionStatus VerticalMotionStatus;

struct Attitude
{
  double roll;
  double pitch;
  double yaw;
};
typedef struct Attitude Attitude;

struct XYZ
{
  double x;
  double y;
  double z;
};
typedef struct XYZ XYZ;

struct XY
{
  double x;
  double y;
};
typedef struct XY XY;

struct GlobalLocation
{
  double latitude;
  double longitude;
  double altitude;
};
typedef struct GlobalLocation GlobalLocation;

struct OrientationQuaternion
{
  double w;
  double x;
  double y;
  double z;
};
typedef struct OrientationQuaternion OrientationQuaternion;

struct Location
{
  LocationStatus locationStatus;
  XYZ localLocation;
  GlobalLocation globalLocation;
  double heading;
  double localHeading;
  XY uncertainty;
  VerticalMotionStatus verticalMotionStatus;
};
typedef struct Location Location;

struct Motion
{
  double stepFrequency;
  PrimaryMotion primaryMotion;
  SecondaryMotion secondaryMotion;
};
typedef struct Motion Motion;

enum MapObjectType
  {
    street = 0,
    building = 1,
    voronoi = 2,
    door = 3,
    store = 4,
    elevator = 5,
    escalator = 6,
    obstacle = 7,
    building_side = 8,
    traversable = 9,
    natural = 10,
    parking = 11,
    ble = 12,
    invalid = -1,
    unknown = -2
  };
typedef enum MapObjectType MapObjectType;

@interface NVSSMessage : NSObject
@property BOOL hasMessageToEnd;
@property BOOL hasMessage;
@property NSString* messageToEnd;
@property NSString* message;
@end

@interface MotionDna : NSObject<NSObject>
{
  @protected
  Location location_;
  Location GPSLocation_;
  Attitude attitude_;
  Motion motion_;
  CalibrationStatus calibrationStatus_;
  NSString * ID_;
  NSString * deviceName_;
  MotionStatistics motionStatistics_;
  MotionStatistics polygonMotionStatistics_;
  OrientationQuaternion quaternion_;
  @public
    NVSSMessage* message_;
}

-(Location)getLocation;
-(Location)getGPSLocation;
-(Attitude)getAttitude;
-(Motion)getMotion;
-(CalibrationStatus)getCalibrationStatus;
-(NSString*)getID;
-(NSString*)getDeviceName;
-(MotionStatistics)getMotionStatistics;
-(MotionStatistics)getPolygonMotionStatistics;
-(OrientationQuaternion)getQuaternion;
-(XY)getDebugVector;
@end

#endif
