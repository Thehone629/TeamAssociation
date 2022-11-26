"""
@FileName：config.py\n
@Description：\n
@Author：NZQ\n
@Time：2022/11/18 16:28\n
"""
HOSTNAME = "localhost"
PORT = "3306"
DATABASE = "test_py"
USERNAME = "root"
PASSWORD = "123456"
DB_URI = 'mysql+pymysql://{}:{}@{}:{}/{}?charset=utf8'.format(HOSTNAME,USERNAME,PORT,PASSWORD,DATABASE)
SQLALCHEMY_DATABASE_URI = DB_URI