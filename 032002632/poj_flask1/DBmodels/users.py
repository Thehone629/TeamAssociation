"""
@FileName：users.py.py\n
@Description：\n
@Author：NZQ\n
@Time：2022/11/18 15:38\n
"""
from sqlalchemy import MetaData, Table

from  main import  db




class User(db.Model):
    __table__ = Table('users',MetaData(bind=db.engine),autoload = True)
    def find_user_by_id(self,userid):
        row = db.session.query(User).filter(User.id ==  userid).first()
        return row
