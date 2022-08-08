using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EP94.ThinqSharp.Config
{
    internal static class ErrorCodes
    {
        public const int OK = 0000;
        public const int PARTIAL_OK = 0001;
        public const int OPERATION_IN_PROGRESS_DEVICE = 0103;
        public const int PORTAL_INTERWORKING_ERROR = 0007;
        public const int PROCESSING_REFRIGERATOR = 0104;
        public const int RESPONSE_DELAY_DEVICE = 0111;
        public const int SERVICE_SERVER_ERROR = 8107;
        public const int SSP_ERROR = 8102;
        public const int TIME_OUT = 9020;
        public const int WRONG_XML_OR_URI = 9000;

        public const int AWS_IOT_ERROR = 8104;
        public const int AWS_S3_ERROR = 8105;
        public const int AWS_SQS_ERROR = 8106;
        public const int BASE64_DECODING_ERROR = 9002;
        public const int BASE64_ENCODING_ERROR = 9001;
        public const int CLIP_ERROR = 8103;
        public const int CONTROL_ERROR_REFRIGERATOR = 0105;
        public const int CREATE_SESSION_FAIL = 9003;
        public const int DB_PROCESSING_FAIL = 9004;
        public const int DM_ERROR = 8101;
        public const int DUPLICATED_ALIAS = 0013;
        public const int DUPLICATED_DATA = 0008;
        public const int DUPLICATED_LOGIN = 0004;
        public const int EMP_AUTHENTICATION_FAILED = 0102;
        public const int ETC_COMMUNICATION_ERROR = 8900;
        public const int ETC_ERROR = 9999;
        public const int EXCEEDING_LIMIT = 0112;
        public const int EXPIRED_CUSTOMER_NUMBER = 0119;
        public const int EXPIRES_SESSION_BY_WITHDRAWAL = 9005;
        public const int FAIL = 0100;
        public const int INACTIVE_API = 8001;
        public const int INSUFFICIENT_STORAGE_SPACE = 0107;
        public const int INVAILD_CSR = 9010;
        public const int INVALID_BODY = 0002;
        public const int INVALID_CUSTOMER_NUMBER = 0118;
        public const int INVALID_HEADER = 0003;
        public const int INVALID_PUSH_TOKEN = 0301;
        public const int INVALID_REQUEST_DATA_FOR_DIAGNOSIS = 0116;
        public const int MISMATCH_DEVICE_GROUP = 0014;
        public const int MISMATCH_LOGIN_SESSION = 0114;
        public const int MISMATCH_NONCE = 0006;
        public const int MISMATCH_REGISTRED_DEVICE = 0115;
        public const int MISSING_SERVER_SETTING_INFORMATION = 9005;
        public const int NOT_AGREED_TERMS = 0110;
        public const int NOT_CONNECTED_DEVICE = 0106;
        public const int NOT_CONTRACT_CUSTOMER_NUMBER = 0120;
        public const int NOT_EXIST_DATA = 0010;
        public const int NOT_EXIST_DEVICE = 0009;
        public const int NOT_EXIST_MODEL_JSON = 0117;
        public const int NOT_REGISTERED_SMART_CARE = 0121;
        public const int NOT_SUPPORTED_COMMAND = 0012;
        public const int NOT_SUPPORTED_COUNTRY = 8000;
        public const int NOT_SUPPORTED_SERVICE = 0005;
        public const int NO_INFORMATION_DR = 0109;
        public const int NO_INFORMATION_SLEEP_MODE = 0108;
        public const int NO_PERMISSION = 0011;
        public const int NO_PERMMISION_MODIFY_RECIPE = 0113;
        public const int NO_REGISTERED_DEVICE = 0101;
        public const int NO_USER_INFORMATION = 9006;
    }
}
