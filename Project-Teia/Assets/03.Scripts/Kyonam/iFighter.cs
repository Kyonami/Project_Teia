public enum Choices
{
    ATTACK,
    DEFENSE,
    SKILL,
    WAIT,
    NONE
}

interface IFighter {
    void Attack();
	void Defend();
	void UseSkill();
    void Wait();
    void ResetChoice();
    void DecreaseHP(int _value);
}