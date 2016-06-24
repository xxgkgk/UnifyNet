using System;
using UnCommon.Tool;

namespace UnCommon.Entity
{

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
        // 添加接收大小
        public void addReceiveLength(long l)
        {
            _receiveSum += l;
        }
        // 添加发送大小
        public void addSendLength(long l)
        {
            _sendSum += l;
        }

        // 计算
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

        // 获取接收速度
        public float getReceiveSpeed()
        {
            calculate();
            return _receiveSpeed;
        }
        // 获取接收峰值
        public float getReceiveSpeedPeak()
        {
            calculate();
            return _receiveSpeedPeak;
        }
        // 获取上行速度
        public float getSendSpeed()
        {
            calculate();
            return _sendSpeed;
        }
        // 获取上行峰值
        public float getSendSpeedPeak()
        {
            calculate();
            return _sendSpeedPeak;
        }

        // UpFile-任务数
        private int _upTaskNum = 0;
        // UpFile-任务数峰值
        private int _upTaskNumPeak = 0;
        // UpFile-添加任务数
        public void addUpTaskNum(int n)
        {
            _upTaskNum += n;
            if (_upTaskNum > _upTaskNumPeak)
            {
                _upTaskNumPeak = _upTaskNum;
            }
        }
        // UpFile-获取当前任务数
        public int getUpTaskNum()
        {
            calculate();
            return _upTaskNum;
        }
        // UpFile-获取任务数峰值
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
        // 添加接收包数
        public void addReceiveNum(int n)
        {
            _receiveNum += n;
        }
        // 获取当前包数
        public int getReceiveNum()
        {
            return _receiveNum;
        }
        // 获取处理速度
        public float getReceiveNumSpeed()
        {
            calculate();
            return _receiveNumSpeed;
        }
        // 获取处理峰值
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
        // 添加处理大小
        public void addProLength(long l)
        {
            _proSum += l;
        }
        // 获取处理速度
        public float getProSpeed()
        {
            calculate();
            return _proSpeed;
        }
        // 获取处理峰值
        public float getProSpeedPeak()
        {
            calculate();
            return _proSpeedPeak;
        }

        // DownFile-任务数
        private int _downTaskNum = 0;
        // DownFile-任务数峰值
        private int _downTaskNumPeak = 0;
        // DownFile-添加任务数
        public void addDownTaskNum(int n)
        {
            _downTaskNum += n;
            if (_downTaskNum > _downTaskNumPeak)
            {
                _downTaskNumPeak = _downTaskNum;
            }
        }
        // DownFile-获取当前任务数
        public int getDownTaskNum()
        {
            calculate();
            return _downTaskNum;
        }
        // DownFile-获取任务数峰值
        public int getDownTaskNumPeak()
        {
            calculate();
            return _downTaskNumPeak;
        }
        // DownFile-缓存数
        private int _downCacheNum = 0;
        // DownFile-缓存数峰值
        private int _downCacheNumPeak = 0;
        // DownFile-添加缓存数
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
        // DownFile-获取当前缓存数
        public int getDownCacheNum()
        {
            return _downCacheNum;
        }
        // DownFile-获取缓存峰值
        public int getDownCacheNumPeak()
        {
            return _downCacheNumPeak;
        }
        // DownFile-缓存数
        private int _downFileNum = 0;
        // DownFile-缓存数峰值
        private int _downFileNumPeak = 0;
        // DownFile-添加缓存数
        public void addDownFileNum(int n)
        {
            _downFileNum += n;
            if (_downFileNum > _downFileNumPeak)
            {
                _downFileNumPeak = _downFileNum;
            }
        }
        // DownFile-获取当前缓存数
        public int getDownFileNum()
        {
            return _downFileNum;
        }
        // DownFile-获取缓存峰值
        public int getDownFileNumPeak()
        {
            return _downFileNumPeak;
        }
    }
}
