namespace MyMicroService.Infrastruct.ResponseModels
{
    /// <summary>
    /// 统一消息返回
    /// </summary>
    public partial class ResponseMessage<TEntity>
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; protected set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public TEntity Data { get; protected set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="Message">消息内容</param>
        public virtual void ReturnMessage(bool isSuccess, string message)
        {
            this.IsSuccess = isSuccess;
            this.Message = message;
        }

        /// <summary>
        /// 返回数据
        /// </summary>
        /// <param name="isSuccess">是否成功</param>
        /// <param name="Data">数据</param>
        public virtual void ReturnData(bool isSuccess, TEntity Data)
        {
            this.IsSuccess = isSuccess;
            this.Data = Data;
        }
    }

    /// <summary>
    /// 统一消息返回
    /// </summary>
    public partial class ResponseMessage : ResponseMessage<byte>
    {
    }
}
