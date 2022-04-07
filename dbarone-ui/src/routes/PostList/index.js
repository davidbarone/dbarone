import { h } from "preact";
import style from "./style.css";
import { useState, useEffect } from "preact/hooks";
import { getPosts } from "../../utils/ApiFacade";
import MyTable from "../../widgets/myTable";

const PostList = () => {
  const [posts, setPosts] = useState([]);
  const [links, setLinks] = useState([]);

  useEffect(() => {
    getPosts().then((p) => {
      setPosts(p.resource);
      setLinks(p.links);
    });
  }, []);

  return (
    <div class={style.home}>
      <h1>Posts</h1>

      <MyTable
        data={posts}
        mapping={{
          "Project Id": (p) => p.resource.id,
          Title: (p) => p.resource.title,
        }}
      />
    </div>
  );
};

export default PostList;
