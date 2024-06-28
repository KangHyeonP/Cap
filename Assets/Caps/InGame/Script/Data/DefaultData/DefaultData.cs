
public enum ECharacters
{
    main, 
    sub, 
    c1, 
    c2, 
    c3
}

// �⺻���� ������ ���� �����ϴ� Ŭ���� ex)���� �÷��̾��� �ɷ�ġ

public class DefaultData
{
    // Characters
    private CharacterData characters;
    public CharacterData Characters => characters;

    // CurIndex
    private ECharacters enumC;

    public DefaultData()
    {
        characters = new CharacterData();
    }

    public void SettingValue(int index)
    {
        characters.UpdateData(index);
    }
}

// �⺻ ĳ���� Ư��
public class CharacterData
{
    // ����, ����, 1, 2, 3 -> Characters, �̸� �� �������
    private int[] defaultHp = { 6, 4, 1, 1, 1 };
    private float[] defaultSpeed = { 3.5f, 4.0f, 2, 2, 2 };
    private float[] defaultAimAccuracy = { 5, 7, 3, 3, 3 };
    private int[] defaultAttackPower = { 10, 5, 4, 4, 4 };
    private float[] defaultAttackDealy = { 0, 0, 0, 0, 0 };

    public void UpdateData(int i)
    {
        if (DataManager.Instacne.JsonClass._PlayerData.playerLock[i])
        GameManager.Instance.GetPlayerValue(defaultHp[i], defaultSpeed[i], defaultAimAccuracy[i], defaultAttackPower[i], defaultAttackDealy[i]);
    }
}
