using Amazon;
using Amazon.Rekognition;
using Amazon.Rekognition.Model;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Mvc;
using Amazon.TranscribeService;
using Amazon.TranscribeService.Model;
using System.Text;

namespace ChatGptExam.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RekognitionController : ControllerBase
    {

        [HttpPost]
        [Route("UploadFiles")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var accessKey = "AKIAXDVZCEHP3ZPGA3RN";
            var secretKey = "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";
            var region = RegionEndpoint.EUWest2;

            var s3Client = new AmazonS3Client(accessKey, secretKey, region);
            var rekognitionClient = new AmazonRekognitionClient(accessKey, secretKey, region);

            var transferUtility = new TransferUtility(s3Client);
            var transferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = "mylast-bucket",
                InputStream = file.OpenReadStream(),
                Key = file.FileName,
                ContentType = file.ContentType
            };

            await transferUtility.UploadAsync(transferUtilityRequest);

            var detectTextRequest = new DetectTextRequest
            {
                Image = new Image
                {
                    S3Object = new S3Object
                    {
                        Bucket = "mylast-bucket",
                        Name = file.FileName
                    }
                }
            };

            var detectTextResponse = await rekognitionClient.DetectTextAsync(detectTextRequest);
            var detectedText = string.Join(", ", detectTextResponse.TextDetections.Select(td => td.DetectedText));

            return Ok(detectedText);
        }
        [HttpPost]
        [Route("Transcribe")]
        public async Task<IActionResult> Transcribe(IFormFile file)
        {
            var accessKey = "AKIAXDVZCEHP3ZPGA3RN";
            var secretKey = "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";
            var region = RegionEndpoint.EUWest2;

            using var transcribeClient = new AmazonTranscribeServiceClient(accessKey, secretKey, region);

            var jobName = "my-transcription-job";
            var languageCode = LanguageCode.EnUS;
            var mediaFormat = MediaFormat.Mp3;

            var transcriptionJobRequest = new StartTranscriptionJobRequest
            {
                TranscriptionJobName = jobName,
                LanguageCode = languageCode,
                MediaFormat = mediaFormat,
                Media = new Media { MediaFileUri = file.FileName }
            };

            var response = await transcribeClient.StartTranscriptionJobAsync(transcriptionJobRequest);

            var jobStatus = "";
            while (jobStatus != TranscriptionJobStatus.COMPLETED)
            {
                var jobResponse = await transcribeClient.GetTranscriptionJobAsync(new GetTranscriptionJobRequest
                {
                    TranscriptionJobName = jobName
                });

                jobStatus = jobResponse.TranscriptionJob.TranscriptionJobStatus;

                await Task.Delay(5000);
            }

            var jobResult = await transcribeClient.GetTranscriptionJobAsync(new GetTranscriptionJobRequest
            {
                TranscriptionJobName = jobName
            });

            var transcriptUrl = jobResult.TranscriptionJob.Transcript.TranscriptFileUri;
            using var httpClient = new HttpClient();
            var transcriptFileBytes = await httpClient.GetByteArrayAsync(transcriptUrl);
            var transcriptionText = Encoding.UTF8.GetString(transcriptFileBytes);

            return Content(transcriptionText);
        }
    }
}
