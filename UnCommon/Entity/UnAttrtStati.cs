using System;
using UnCommon.Tool;

namespace UnCommon.Entity
{
    /// <summary>
    /// 统计类
    /// </summary>
    public class UnAttrtStati
    {
        // 统计时间戳
        private decimal ticks = 0;

        // 接收累加
        private long _receiveSum = 0;
        // 接收速度
        private float _receiveSpeed = 0;
        // 接收峰值
        private float _receiveSpeedPeak = 0;

        // 上行累加
        private long _sendSum = 0;
        // 上行速度
        private float _sendSpeed = 0;
        // 上行峰值
        private float _sendSpeedPeak = 0;
        /// <summary>
        /// 添加接收大小
        /// </summary>
        /// <param name="l"></param>
        public void addReceiveLength(long l)
        {
            _receiveSum += l;
        }
        /// <summary>
        /// 添加发送大小
        /// </summary>
        /// <param name="l"></param>
        public void addSendLength(long l)
        {
            _sendSum += l;
        }

        /// <summary>
        /// 计算
        /// </summary>
        private void calculate()
        {
            decimal space = UnDate.ticksSec() - ticks;
            if (space > 5)
            {
                this._receiveSpeed = (float)Math.Round((this._receiveSum / (space * 1024)), 2);
                this._receiveNumSpeed = (float)Math.Round((this._receiveNum / space), 2);
                this._sendSpeed = (float)Math.Round((this._sendSum / (space * 1024)), 2);
                this._proSpeed = (float)Math.Round((this._proSum / (space * 1024)), 2);
                if (this._receiveSpeed > this._receiveSpeedPeak)
                {
                    this._receiveSpeedPeak = this._receiveSpeed;
                }
                if (this._receiveNumSpeed > this._receiveNumSpeedPeak)
                {
                    this._receiveNumSpeedPeak = this._receiveNumSpeed;
                }
                if (this._sendSpeed > this._sendSpeedPeak)
                {
                    this._sendSpeedPeak = this._sendSpeed;
                }
                if (this._proSpeed > this._proSpeedPeak)
                {
                    this._proSpeedPeak = this._proSpeed;
                }
                this.ticks = UnDate.ticksSec();
                this._receiveSum = 0;
                this._receiveNum = 0;
                this._sendSum = 0;
                this._proSum = 0;
            }
        }

        /// <summary>
        /// 获取接收速度
        /// </summary>
        /// <returns></returns>
        public float getReceiveSpeed()
        {
            calculate();
            return _receiveSpeed;
        }
        /// <summary>
        /// 获取接收峰值
        /// </summary>
        /// <returns></returns>
        public float getReceiveSpeedPeak()
        {
            calculate();
            return _receiveSpeedPeak;
        }
        /// <summary>
        /// 获取上行速度
        /// </summary>
        /// <returns></returns>
        public float getSendSpeed()
        {
            calculate();
            return _sendSpeed;
        }
        /// <summary>
        /// 获取上行峰值
        /// </summary>
        /// <returns></returns>
        public float getSendSpeedPeak()
        {
            calculate();
            return _sendSpeedPeak;
        }

        // UpFile-任务数
        private int _upTaskNum = 0;
        // UpFile-任务数峰值
        private int _upTaskNumPeak = 0;
        /// <summary>
        /// UpFile-添加任务数
        /// </summary>
        /// <param name="n"></param>
        public void addUpTaskNum(int n)
        {
            _upTaskNum += n;
            if (_upTaskNum > _upTaskNumPeak)
            {
                _upTaskNumPeak = _upTaskNum;
            }
        }
        /// <summary>
        /// UpFile-获取当前任务数
        /// </summary>
        /// <returns></returns>
        public int getUpTaskNum()
        {
            calculate();
            return _upTaskNum;
        }
        /// <summary>
        /// UpFile-获取任务数峰值
        /// </summary>
        /// <returns></returns>
        public int getUpTaskNumPeak()
        {
            calculate();
            return _upTaskNumPeak;
        }

        // 接收包数
        private int _receiveNum = 0;
        // 接收包峰值
        private float _receiveNumSpeed = 0;
        // 接收包速度峰值
        private float _receiveNumSpeedPeak = 0;
        /// <summary>
        /// 添加接收包数
        /// </summary>
        /// <param name="n"></param>
        public void addReceiveNum(int n)
        {
            _receiveNum += n;
        }
        /// <summary>
        /// 获取当前包数
        /// </summary>
        /// <returns></returns>
        public int getReceiveNum()
        {
            return _receiveNum;
        }
        /// <summary>
        /// 获取处理速度
        /// </summary>
        /// <returns></returns>
        public float getReceiveNumSpeed()
        {
            calculate();
            return _receiveNumSpeed;
        }
        /// <summary>
        /// 获取处理峰值
        /// </summary>
        /// <returns></returns>
        public float getReceiveNumSpeedPeak()
        {
            calculate();
            return _receiveNumSpeedPeak;
        }

        // 处理累加
        private long _proSum = 0;
        // 处理速度
        private float _proSpeed = 0;
        // 处理峰值
        private float _proSpeedPeak = 0;

        /// <summary>
        /// 添加处理大小
        /// </summary>
        /// <param name="l"></param>
        public void addProLength(long l)
        {
            _proSum += l;
        }
        /// <summary>
        /// 获取处理速度
        /// </summary>
        /// <returns></returns>
        public float getProSpeed()
        {
            calculate();
            return _proSpeed;
        }
        /// <summary>
        /// 获取处理峰值
        /// </summary>
        /// <returns></returns>
        public float getProSpeedPeak()
        {
            calculate();
            return _proSpeedPeak;
        }

        /// <summary>
        /// DownFile-任务数
        /// </summary>
        private int _downTaskNum = 0;
        // DownFile-任务数峰值
        private int _downTaskNumPeak = 0;
        /// <summary>
        /// DownFile-添加任务数
        /// </summary>
        /// <param name="n"></param>
        public void addDownTaskNum(int n)
        {
            _downTaskNum += n;
            if (_downTaskNum > _downTaskNumPeak)
            {
                _downTaskNumPeak = _downTaskNum;
            }
        }
        /// <summary>
        /// DownFile-获取当前任务数
        /// </summary>
        /// <returns></returns>
        public int getDownTaskNum()
        {
            calculate();
            return _downTaskNum;
        }
        /// <summary>
        /// DownFile-获取任务数峰值
        /// </summary>
        /// <returns></returns>
        public int getDownTaskNumPeak()
        {
            calculate();
            return _downTaskNumPeak;
        }
        // DownFile-缓存数
        private int _downCacheNum = 0;
        /// <summary>
        /// DownFile-缓存数峰值
        /// </summary>
        private int _downCacheNumPeak = 0;
        /// <summary>
        /// DownFile-添加缓存数
        /// </summary>
        /// <param name="n"></param>
        public void addDownCacheNum(int n)
        {
            _downCacheNum += n;
            if (_downCacheNum < 0)
            {
                _downCacheNum = 0;
            }
            if (_downCacheNum > _downCacheNumPeak)
            {
                _downCacheNumPeak = _downCacheNum;
            }
        }
        /// <summary>
        /// DownFile-获取当前缓存数
        /// </summary>
        /// <returns></returns>
        public int getDownCacheNum()
        {
            return _downCacheNum;
        }
        /// <summary>
        /// DownFile-获取缓存峰值
        /// </summary>
        /// <returns></returns>
        public int getDownCacheNumPeak()
        {
            return _downCacheNumPeak;
        }
        // DownFile-缓存数
        private int _downFileNum = 0;
        // DownFile-缓存数峰值
        private int _downFileNumPeak = 0;
        /// <summary>
        /// DownFile-添加缓存数
        /// </summary>
        /// <param name="n"></param>
        public void addDownFileNum(int n)
        {
            _downFileNum += n;
            if (_downFileNum > _downFileNumPeak)
            {
                _downFileNumPeak = _downFileNum;
            }
        }
        /// <summary>
        /// DownFile-获取当前缓存数
        /// </summary>
        /// <returns></returns>
        public int getDownFileNum()
        {
            return _downFileNum;
        }
        /// <summary>
        ///  DownFile-获取缓存峰值
        /// </summary>
        /// <returns></returns>
        public int getDownFileNumPeak()
        {
            return _downFileNumPeak;
        }
    }
}
