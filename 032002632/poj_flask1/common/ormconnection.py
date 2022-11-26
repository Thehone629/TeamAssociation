"""
@FileName：ormconnection.py\n
@Description：\n
@Author：NZQ\n
@Time：2022/11/18 14:43\n
"""

from sqlalchemy import create_engine, MetaData, Table
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker, session, scoped_session

engin = create_engine('mysql+pymysql://root:123456@localhost/test_py')
DBsession = sessionmaker(bind=engin)
dbsession = scoped_session(DBsession)

Base = declarative_base()
md = MetaData(bind=engin)


class User(Base):
    __table__ = Table('users', md, autoload=True)



if __name__ == '__main__':
    pass
    # result = dbsession.query(User).filter(User.id<=10).all()
    # result1 = dbsession.query(User).filter_by(id=6).all()
    # for i in result:
    #     print(i.id)
    #     print(i.name)
    #     print(i.password)
    # for i in result1:
    #     print(i.name)
    #为一个数据时 返回的不是列表，不可迭代
    # result_row = dbsession.query(User).filter(User.id >= 10).first()
    # # print(result[1].name)
    # print(result_row.name)

    # ------------新增 add----------
    # user = User(id = "idtest",name = "kk",password = "pwd")
    # dbsession.add(user)
    # dbsession.commit()


    #-----------更新------------
    # row = dbsession.query(User).filter(User.id == "idtest").first()
    # print(row.name)
    # row.name = "newid"
    # #直接提交就行不用再添加
    # dbsession.commit()
    # row = dbsession.query(User).filter(User.id == "idtest").first()
    # print(row.name)


    # -----------删除------------
    # row = dbsession.query(User).filter(User.id == "idtest").delete()
    # dbsession.commit()

