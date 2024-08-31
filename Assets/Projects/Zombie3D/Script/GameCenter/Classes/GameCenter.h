
#ifndef _GAME_CENTER_H_
#define _GAME_CENTER_H_


#include <string>


class GameCenterImpl;


namespace Triniti2D {


//! @class GameCenter
//! @brief GameCenter
class GameCenter
{
public:
	//! ��½״̬
	enum LOGIN_STATUS
	{
		LOGIN_STATUS_IDLE = 0,		//!< ����
		LOGIN_STATUS_WAIT,			//!< �ȴ�
		LOGIN_STATUS_SUCCESS,		//!< �ɹ�
		LOGIN_STATUS_ERROR			//!< ����
	};

	//! �ύ״̬
	enum SUBMIT_STATUS
	{
		SUBMIT_STATUS_IDLE = 0,		//!< ����
		SUBMIT_STATUS_WAIT,			//!< �ȴ�
		SUBMIT_STATUS_SUCCESS,		//!< �ɹ�
		SUBMIT_STATUS_ERROR,		//!< ����
	};

public:
	//! ��ʼ��
	static void Initialize();

	//! ����ʼ��
	static void Uninitialize();

public:
	//! ��ǰ�豸�Ƿ�֧��GameCenter
	static bool IsSupported();

public:
	//! �Ƿ��½GameCenter
	static bool IsLogin();

	//! ��½GameCenter
	static bool Login();

	//! ��½״̬
	static LOGIN_STATUS LoginStatus();

public:
	//! ��ȡ�ʺ�
	static bool GetAccount(std::string& account);

	//! ��ȡ����
	static bool GetName(std::string& name);

public:
	//! �ύ����
	//! @param category ���а�id
	//! @param score ����
	static bool SubmitScore(const std::string& category, int score);

	//! �ύ����״̬
	//! @param category ���а�id
	//! @param score ����
	static SUBMIT_STATUS SubmitScoreStatus(const std::string& category, int score);

	//! �ύ�ɾ�
	//! @param category ���а�id
	//! @param percent �ٷֱ�
	static bool SubmitAchievement(const std::string& category, int percent);

	//! �ύ�ɾ�״̬
	//! @param category ���а�id
	//! @param percent �ٷֱ�
	static SUBMIT_STATUS SubmitAchievementStatus(const std::string& category, int percent);

public:
	//! ��������
	static bool OpenLeaderboard();

	//! ��������
	static bool OpenLeaderboard(const std::string& category);

	//! �򿪳ɾͿ�
	static bool OpenAchievement();

private:
	//! ʵ��
	static GameCenterImpl* m_impl;
};


} // namespace Triniti2D


#endif // _GAME_CENTER_H_
