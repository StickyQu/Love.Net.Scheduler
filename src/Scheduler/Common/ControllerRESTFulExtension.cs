namespace Microsoft.AspNetCore.Mvc
{
    public static class ControllerRESTFulExtension {
        public static UnprocessableEntityResult UnprocessableEntity(this Controller controller, object value = null) {
            return new UnprocessableEntityResult(value);
        }
    }
}