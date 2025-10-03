static bool IsPalindrome(string text)
{
    int left = 0;
    int right = text.Length - 1;

    while (left < right)
    {
        if (text[left] != text[right])
        {
            return false;
        }
        left++;
        right--;
    }

    return true;
}
////////////////////////////////////////////
static int MinSplit(int amount)
{
    int[] coins = { 50, 20, 10, 5, 1 };
    int count = 0;

    foreach (int coin in coins)
    {
        count += amount / coin;
        amount = amount % coin;
    }

    return count;
}
////////////////////////////////////////
static int FindMinPositive(int[] array)
{

    int result = 1;


    while (true)
    {

        bool found = false;

        foreach (int num in array)
        {
            if (num == result)
            {
                found = true;
                break;
            }
        }

        if (!found)
        {
            return result;
        }


        result++;
    }
}
    }

//////////////////////////////////
    static bool IsProperly(string sequence)
{
    int counter = 0;

    foreach (char c in sequence)
    {
        if (c == '(')
        {
            counter++;
        }
        else if (c == ')')
        {
            counter--;
        }

        if (counter < 0)
        {
            return false;
        }
    }

    return counter == 0;
}
}
//////////////////////////////////////////////////////
static int CountWaysRecursive(int n)
{

    if (n == 1)
    {
        return 1;
    }
    if (n == 2)
    {
        return 2;
    }


    return CountWaysRecursive(n - 1) + CountWaysRecursive(n - 2);
}
///////////////////////////////////////////////////////////////////////////
create table Teacher(
TeacherID int primary key,
FirstName varchar(50),
LastName varchar(50),
Gender varchar(10),
Subjectt  varchar(50)
);
create table Pupil (
PupilId int primary key,
FirstName Varchar(50),
LastName varchar(50),
Gender varchar(10),
Class varchar (10)
);
create table TeacherPupil (
TeacherId int,
PupilId int,
primary key (TeacherId, PupilId),
foreign key (TeacherId) references Teacher(TeacherId),
foreign key (PupilId) references Pupil(PupilId)
);


SELECT t.FirstName, t.LastName
FROM Teacher t
JOIN TeacherPupil tp ON t.TeacherId = tp.TeacherId
JOIN Pupil p ON tp.PupilId = p.PupilId
WHERE p.FirstName = 'გიორგი';
///////////////////////////////////////////////////////////////////
